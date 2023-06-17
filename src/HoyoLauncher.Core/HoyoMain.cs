namespace HoyoLauncher.Core;

public sealed class HoyoMain
{
    private static bool _isgamerunning;
    public static bool IsGameRunning
    { 
        get => _isgamerunning;
        set
        {
            _isgamerunning = value;

            UpdateIsGameRunning();
        }
    }

    public static bool FirstRun { get; set; }
    public static HoyoGames CurrentGameSelected { get; set; }
    public static string ExecutableName { get; private set; }

    public static void Initialize()
    {
        UpdateConfig();

        FirstRun = true;
        CurrentGameSelected = HoyoGames.DEFAULT;
        IsGameRunning = false;

        EventsAttribute.SetEvents();

        ValidateSettings(AppSettings.Settings.Default.GENSHIN_IMPACT_DIR, HoyoGames.GenshinImpact, HoyoWindow.GENSHIN_IMPACT_LAUNCHER, out bool _);
        ValidateSettings(AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR, HoyoGames.HonkaiStarRail, HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER, out bool _);
        ValidateSettings(AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR, HoyoGames.HonkaiImpactThird, HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER, out bool _);

        LastGame();
    }

    public static void GameChange(string uid) =>
        GameChange(short.Parse(uid));
    public static void GameChange(short uid)
    {
        ConfigRead GameConfig = ConfigRead.GetConfig(CurrentGameSelected.GAME_DIRECTORY);
        
        ImageBrush GameBG = 
            GameConfig.GameBackground is null ? CurrentGameSelected.GAME_DEFAULT_BG : GameConfig.GameBackground;

        HoyoValues values = new() 
        {
            Background = GameBG,
            RemoveMainBG = true,
            CheckInButton = true,
            LaunchButton = true,
            LaunchButtonContent = AppResources.Resources.GAME_DEFAULT_TEXT     
        };

        if(CurrentGameSelected == HoyoGames.ZenlessZoneZero)
        {
            values.CheckInButton =
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_SOON_TEXT;
        }
        else if(!GameConfig.ConfigExist)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_NOTFOUND;
        }

        if(IsGameRunning)
        {
            values.LaunchButton = false;
            values.LaunchButtonContent = AppResources.Resources.GAME_LAUNCHED_TEXT;
        }

        values.ApplyChanges();
        
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

        // ahh goofy ☠️
        if
        (
            !(ErrorOccured = !GameConfigData.ConfigExist || (Path.GetFileName(GameConfigData.GameStartName) != Game.GAME_EXECUTABLE))
        )
        {
            LauncherButton.IsEnabled = true;
            return;
        }

        LauncherButton.IsEnabled = false;
    }

    // Sets to the last game selected
    static void LastGame()
    {
        HoyoGames SelectedHoyoGame = null;
        short uid = AppSettings.Settings.Default.LAST_GAME;

        switch(uid)
        {
            case 1: SelectedHoyoGame = HoyoGames.GenshinImpact; break;
            case 2: SelectedHoyoGame = HoyoGames.HonkaiStarRail; break;
            case 3: SelectedHoyoGame = HoyoGames.HonkaiImpactThird; break;
        }

        if(SelectedHoyoGame is not null)
        {
            CurrentGameSelected = SelectedHoyoGame;
            GameChange(--uid);
        }
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

            HoyoWindow.Loaded += async (s, e) =>
            {
                await Task.Delay(1000);
                new ShortTour { Owner = HoyoWindow }.ShowDialog();
            };

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

    // keeps fake builds that might be injected with malware.
    public static string GenerateMD5HASH()
    {
        using var md5 = System.Security.Cryptography.MD5.Create();

        #if DEBUG
            using var stream = File.OpenRead(Assembly.GetExecutingAssembly().Location);
        #else
            using var stream = File.OpenRead(Environment.ProcessPath);
        #endif

        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","");
    }

    private static void UpdateIsGameRunning()
    {
        if (CurrentGameSelected == HoyoGames.DEFAULT) return;

        HoyoWindow.HomeButton.IsEnabled = !IsGameRunning;

        HoyoWindow.LaunchButton.IsEnabled = false;
        HoyoWindow.LaunchButton.Content = AppResources.Resources.GAME_LAUNCHED_TEXT;

        if (IsGameRunning) return;

        ConfigRead GameConfig = ConfigRead.GetConfig(CurrentGameSelected.GAME_DIRECTORY);

        string LaunchButtonContent = string.Empty switch
        {
            _ when CurrentGameSelected == HoyoGames.ZenlessZoneZero
                => AppResources.Resources.GAME_SOON_TEXT,

            _ when !GameConfig.ConfigExist && CurrentGameSelected != HoyoGames.ZenlessZoneZero
                => AppResources.Resources.GAME_NOTFOUND,

            _ => AppResources.Resources.GAME_DEFAULT_TEXT
        };

        if(LaunchButtonContent == AppResources.Resources.GAME_DEFAULT_TEXT)
            HoyoWindow.LaunchButton.IsEnabled = true;

        HoyoWindow.LaunchButton.Content = LaunchButtonContent;
    }

}