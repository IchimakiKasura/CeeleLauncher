namespace HoyoLauncher.Core;

public sealed class HoyoMain
{
    public static bool FirstRun { get; set; }
    public static bool IsGameRunning { get; set; }
    public static string ExecutableName { get; set; }
    public static HoyoGames CurrentGameSelected { get; private set; }

    public static void Initialize()
    {
        UpdateConfig();

        bool ErrorOccured = false;

        FirstRun = true;
        CurrentGameSelected = HoyoGames.DEFAULT;
        IsGameRunning = false;

        EventsAttribute.SetEvents();

        List<(string config, HoyoGames AbsoluteName, HoyoButton Launcher)> GameConfigs = new()
        {
            (AppSettings.Settings.Default.GENSHIN_IMPACT_DIR, HoyoGames.GenshinImpact, HoyoWindow.GENSHIN_IMPACT_LAUNCHER),
            (AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR, HoyoGames.HonkaiStarRail, HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER),
            (AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR, HoyoGames.HonkaiImpactThird, HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER)
        };

        foreach(var (config, name, Launcher) in CollectionsMarshal.AsSpan(GameConfigs))
        {
            ValidateSettings(config, name, Launcher, out ErrorOccured);
            if(ErrorOccured) break;
        }

        if(!ErrorOccured)
            LastGame();
    }

    public static void GameChange(HoyoGames GS, string uid) =>
        GameChange(GS, short.Parse(uid));

    public static void GameChange(HoyoGames GameSelected, short uid)
    {
        ConfigRead GameConfig = ConfigRead.GetConfig(GameSelected.GAME_DIRECTORY);
        
        ImageBrush GameBG = 
            GameConfig.GameBackground is null ? GameSelected.GAME_DEFAULT_BG : GameConfig.GameBackground;

        HoyoValues values = new() 
        {
            Background = GameBG,
            RemoveMainBG = true,
            CheckInButton = true,
            LaunchButton = true,
            LaunchButtonContent = AppResources.Resources.GAME_DEFAULT_TEXT     
        };

        if(GameSelected == HoyoGames.ZenlessZoneZero)
        {
            values.CheckInButton =
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_SOON_TEXT;
        }

        if(!GameConfig.ConfigExist && GameSelected != HoyoGames.ZenlessZoneZero)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_NOTFOUND;
        }

        values.ApplyChanges();
        
        CurrentGameSelected = GameSelected;
        ExecutableName = GameConfig.GameStartName;

        AppSettings.Settings.Default.LAST_GAME = uid += 1;
        AppSettings.Settings.Default.Save();
    }

    public static void ValidateSettings(string GameConfigName, HoyoGames Game, HoyoButton LauncherButton, out bool ErrorOccured)
    {
        ErrorOccured = false;

        ConfigRead GameConfigData = ConfigRead.GetConfig(GameConfigName); 

        if(GameConfigData.FilePathNone)
        {
            LauncherButton.IsEnabled = false;
            return;
        }

        ErrorOccured = !GameConfigData.ConfigExist || (Path.GetFileName(GameConfigData.GameStartName) != Game.GAME_EXECUTABLE);

        if(!ErrorOccured)
        {
            LauncherButton.IsEnabled = true;
            return;
        }

        if(!FirstRun)
            MessageBox.Show($"ERROR:\n\nThe \"{Game.GAME_NAME}\" cannot be found!\n or its an incorrect game.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        
        LauncherButton.IsEnabled = false;
    }

    // Sets to the last game selected
    static void LastGame()
    {
        HoyoGames Hoyogame = null;
        short uid = AppSettings.Settings.Default.LAST_GAME;

        switch(uid)
        {
            case 1: Hoyogame = HoyoGames.GenshinImpact; break;
            case 2: Hoyogame = HoyoGames.HonkaiStarRail; break;
            case 3: Hoyogame = HoyoGames.HonkaiImpactThird; break;
        }

        if(Hoyogame is not null)
            GameChange(Hoyogame, --uid);
    }

    // Updates the settings on the run
    // using the old version settings and copying it into new version.
    //
    // This prevents when editing the current settings it keeps
    // using the old version settings unless the old version is deleted.
    static void UpdateConfig()
    {
        if(!AppSettings.Settings.Default.FIRSTRUN)
        {
            AppSettings.Settings.Default.Upgrade();
            AppSettings.Settings.Default.FIRSTRUN = true;
        }
    }

    // boilerplate
    public static void ProcessStart(string FileName) =>
        Process.Start(
            new ProcessStartInfo()
            {
                FileName = FileName,
                UseShellExecute = true
            }
        ).Dispose();
}