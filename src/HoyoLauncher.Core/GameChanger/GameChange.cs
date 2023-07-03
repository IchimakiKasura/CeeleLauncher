namespace HoyoLauncher.Core.GameChanger;

public sealed partial class GameChange : HoyoMain
{
    readonly static HoyoValues TempValues = new()
    {
        RemoveMainBG = true,
        LaunchButton = true,
        LaunchButtonContent = LaunchText.GAME_DEFAULT_TEXT,
        VersionBubble = Visibility.Collapsed,
        PreInstall = Visibility.Collapsed
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
}