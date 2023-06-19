namespace HoyoLauncher.Core.GameChanger;

public sealed class GameChange : HoyoMain
{
    readonly static HoyoValues TempValues = new()
    {
        RemoveMainBG = true,
        LaunchButton = true,
        LaunchButtonContent = AppResources.Resources.GAME_DEFAULT_TEXT,
        VersionBubble = Visibility.Hidden
    };

    public static void SetGame(string uid) =>
        SetGame(short.Parse(uid));

    public static void SetGame(short uid)
    {
        HoyoValues values = new(TempValues);
        ConfigRead GameConfig = ConfigRead.GetConfig(CurrentGameSelected.GAME_DIRECTORY);
        ImageBrush GameBG = GameConfig.GameBackground is null ? CurrentGameSelected.GAME_DEFAULT_BG : GameConfig.GameBackground;

        values.Background = GameBG;
        ExecutableName = GameConfig.GameStartName;

        bool[] ConditionMet = 
        {
            CurrentGameSelected.GAME_RESOURCE_API_LINK != "",
            System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable(),
            CurrentGameSelected.GAME_DIR_VALID
        };

        if (ConditionMet.All(x=>x))
        {
            var GameAPI = CurrentGameSelected.API_CACHE ??= new RetrieveAPI(CurrentGameSelected.GAME_CONTENT_API_LINK, CurrentGameSelected.GAME_RESOURCE_API_LINK);

            if(GameAPI.LatestVersion == "CONNECTION FALIURE")
                CurrentGameSelected.API_CACHE = null;

            if (GameConfig.GameVersion != GameAPI.LatestVersion)
            {
                HoyoWindow.VERSION_TEXT.Text = GameAPI.LatestVersion;

                ExecutableName = Path.Combine(CurrentGameSelected.GAME_DIRECTORY, "launcher.exe");

                values.VersionBubble = Visibility.Visible;
                values.LaunchButtonContent = AppResources.Resources.GAME_UPDATE_TEXT;
            }

            if (GameConfig.GameBackgroundMD5 != GameAPI.BackgroundHASH)
                values.Background = new ImageBrush(new BitmapImage(new(GameAPI.BackgroundLINK))) ?? GameBG;
        }
        else
        {
            values.Background = GameBG;
            values.VersionBubble = Visibility.Hidden;
            values.LaunchButtonContent = "NO INTERNET";
        }

        if (CurrentGameSelected == HoyoGames.ZenlessZoneZero)
        {
            values.LaunchButton = false;
            values.Background = CurrentGameSelected.GAME_DEFAULT_BG;
            values.LaunchButtonContent = AppResources.Resources.GAME_SOON_TEXT;
        } 
        else if (!GameConfig.ConfigExist || GameConfig.GameName != CurrentGameSelected.GAME_EXECUTABLE)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_NOTFOUND;
        }

        if (IsGameRunning)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_LAUNCHED_TEXT;
        }

        values.ApplyChanges();

        AppSettings.Settings.Default.LAST_GAME = uid += 1;
        AppSettings.Settings.Default.Save();
    }

}