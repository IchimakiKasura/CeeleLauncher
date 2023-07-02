namespace HoyoLauncher.Core.GameChanger;

public sealed class GameChange : HoyoMain
{
    readonly static HoyoValues TempValues = new()
    {
        RemoveMainBG = true,
        LaunchButton = true,
        LaunchButtonContent = LaunchText.GAME_DEFAULT_TEXT,
        VersionBubble = Visibility.Hidden
    };

    public static async void SetGame(short uid)
    {
        HoyoWindow.LaunchButton.Content = "Loading";
        HoyoWindow.LaunchButton.IsEnabled = false;
        
        HoyoValues values = new(TempValues);

        GameConfigRead GameConfig = CurrentGameSelected.GAME_CONFIG_CACHE;
        ImageBrush GameBG = GameConfig.GameBackground is null ? CurrentGameSelected.GAME_DEFAULT_BG : GameConfig.GameBackground;
        CurrentGameSelected.GAME_INSTALL_PATH ??= GameConfig.GameInstallPath;

        values.Background = GameBG;
        ExecutableName = GameConfig.GameStartName;

        bool[] ConditionMet = 
        {
            CurrentGameSelected.GAME_RESOURCE_API_LINK != "",
            NetworkInterface.GetIsNetworkAvailable(),
            CurrentGameSelected.GAME_DIR_VALID
        };

        if (ConditionMet.All(x => x))
            values = await FetchAPI(GameConfig, values, GameBG);
        else ConnectionFailure(ref values, GameBG);

        if (CurrentGameSelected == HoyoGames.ZenlessZoneZero || CurrentGameSelected == HoyoGames.TearsOfThemis)
        {
            bool IsZZZ = CurrentGameSelected == HoyoGames.ZenlessZoneZero;

            values.Background = CurrentGameSelected.GAME_DEFAULT_BG;
            values.LaunchButton = !IsZZZ;
            values.LaunchButtonContent = IsZZZ ? LaunchText.GAME_SOON_TEXT : LaunchText.GAME_MOBILE;
        } 
        else if (!GameConfig.ConfigExist || GameConfig.GameName != CurrentGameSelected.GAME_EXECUTABLE)
        {
            values.Background = CurrentGameSelected.GAME_DEFAULT_BG;
            values.LaunchButton = false;
            values.LaunchButtonContent = LaunchText.GAME_NOTFOUND;
        }

        if (IsGameRunning)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = LaunchText.GAME_LAUNCHED_TEXT;
        }

        values.ApplyChanges();

        App.Config.LAST_GAME = uid += 1;
        App.Config.SaveConfig();
    }

    static void ConnectionFailure(ref HoyoValues values, ImageBrush GameBG)
    {
        values.Background = GameBG;
        values.VersionBubble = Visibility.Hidden;
        values.LaunchButtonContent = LaunchText.GAME_NO_INTERNET_TEXT;
    }

    static async Task<HoyoValues> FetchAPI(GameConfigRead GameConfig, HoyoValues values, ImageBrush GameBG)
    {
        var WarningMessage = CurrentGameSelected.API_CACHE is null;

        HoyoWindow.VERSION_TEXT.Foreground = Brushes.Black;
        HoyoWindow.VERSION_TEXT.FontWeight = FontWeights.SemiBold;

        // Fetch once and never fetch again (until app was restarted)
        var GameAPI = CurrentGameSelected.API_CACHE ??= await RetrieveAPI.Fetch(CurrentGameSelected.GAME_CONTENT_API_LINK, CurrentGameSelected.GAME_RESOURCE_API_LINK);

        if (GameConfig.GameVersion != GameAPI.LatestVersion)
        {
            HoyoWindow.VERSION_TEXT.Text = GameAPI.LatestVersion;

            ExecutableName = Path.Combine(CurrentGameSelected.GAME_DIRECTORY, "launcher.exe");

            values.VersionBubble = Visibility.Visible;
            values.LaunchButtonContent = LaunchText.GAME_UPDATE_TEXT;
        }

        if (GameConfig.GameBackgroundMD5 != GameAPI.BackgroundHASH)
        {
            values.Background = GameAPI.BackgroundLINK ??= GameBG;

            if (WarningMessage)
                HoyoMessageBox.Show(HoyoWindow.Title, "Its Recommended that you open the Original Launcher To fetch its Background permanently.", HoyoWindow);
        }

        if (GameAPI.LatestVersion == "CONNECTION FAILURE, PLEASE RETRY AGAIN")
        {
            ConnectionFailure(ref values, GameBG);
            values.VersionBubble = Visibility.Visible;
            values.LaunchButton = false;
            HoyoWindow.VERSION_TEXT.Foreground = Brushes.Red;
            HoyoWindow.VERSION_TEXT.FontWeight = FontWeights.Bold;
            CurrentGameSelected.API_CACHE = null;
        }

        if (GameAPI is { DownloadFile: not null } && File.Exists(Path.Combine(CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(GameAPI.DownloadFile.LocalPath))))
        {
            values.VersionBubble = Visibility.Collapsed;
            values.LaunchButtonContent = LaunchText.GAME_EXTRACT_TEXT;
        }

        return values;
    }
}