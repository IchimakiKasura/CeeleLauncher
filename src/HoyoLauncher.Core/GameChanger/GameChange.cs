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

    public static void SetGame(string uid) =>
        SetGame(short.Parse(uid));

    public static async void SetGame(short uid)
    {
        HoyoValues values = new(TempValues);

        ConfigRead GameConfig = ConfigRead.GetConfig(CurrentGameSelected.GAME_DIRECTORY);
        ImageBrush GameBG = GameConfig.GameBackground is null ? CurrentGameSelected.GAME_DEFAULT_BG : GameConfig.GameBackground;
        
        void ConnectionFailure()
        {
            values.Background = GameBG;
            values.VersionBubble = Visibility.Hidden;
            values.LaunchButtonContent = LaunchText.GAME_NO_INTERNET_TEXT;
        }

        values.Background = GameBG;
        ExecutableName = GameConfig.GameStartName;

        bool[] ConditionMet = 
        {
            CurrentGameSelected.GAME_RESOURCE_API_LINK != "",
            System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable(),
            CurrentGameSelected.GAME_DIR_VALID
        };

        if (ConditionMet.All(x => x))
        {
            HoyoWindow.VERSION_TEXT.Foreground = Brushes.Black;
            HoyoWindow.VERSION_TEXT.FontWeight = FontWeights.SemiBold;

            var GameAPI = CurrentGameSelected.API_CACHE ??= await RetrieveAPI.Fetch(CurrentGameSelected.GAME_CONTENT_API_LINK, CurrentGameSelected.GAME_RESOURCE_API_LINK);

            if (GameConfig.GameVersion != GameAPI.LatestVersion)
            {
                HoyoWindow.VERSION_TEXT.Text = GameAPI.LatestVersion;

                ExecutableName = Path.Combine(CurrentGameSelected.GAME_DIRECTORY, "launcher.exe");

                values.VersionBubble = Visibility.Visible;
                values.LaunchButtonContent = LaunchText.GAME_UPDATE_TEXT;
            }

            if (GameConfig.GameBackgroundMD5 != GameAPI.BackgroundHASH)
                values.Background = GameAPI.BackgroundLINK ??= GameBG;

            if (GameAPI.LatestVersion == "CONNECTION FAILURE, PLEASE RETRY AGAIN")
            {
                ConnectionFailure();
                values.VersionBubble = Visibility.Visible;
                values.LaunchButton = false;
                HoyoWindow.VERSION_TEXT.Foreground = Brushes.Red;
                HoyoWindow.VERSION_TEXT.FontWeight = FontWeights.Bold;
                CurrentGameSelected.API_CACHE = null;
            }
        }
        else ConnectionFailure();

        if (CurrentGameSelected == HoyoGames.ZenlessZoneZero)
        {
            values.LaunchButton = false;
            values.Background = CurrentGameSelected.GAME_DEFAULT_BG;
            values.LaunchButtonContent = LaunchText.GAME_SOON_TEXT;
        } 
        else if (!GameConfig.ConfigExist || GameConfig.GameName != CurrentGameSelected.GAME_EXECUTABLE)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = LaunchText.GAME_NOTFOUND;
        }

        if (IsGameRunning)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = LaunchText.GAME_LAUNCHED_TEXT;
        }

        values.ApplyChanges();

        AppSettings.Settings.Default.LAST_GAME = uid += 1;
        AppSettings.Settings.Default.Save();
    }

}