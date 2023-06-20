namespace HoyoLauncher.Core;

public class HoyoMain
{
    private static bool _isgamerunning = false;
    public static bool IsGameRunning
    {
        get => _isgamerunning;
        set
        {
            _isgamerunning = value;

            UpdateIsGameRunning();
        }
    }

    public static bool FirstRun { get; set; } = true;
    public static HoyoGames CurrentGameSelected { get; set; } = HoyoGames.DEFAULT;
    public static string ExecutableName { get; set; }

    public static void Initialize()
    {
        UpdateConfig();

        EventsAttribute.SetEvents();

        ValidateSettings(AppSettings.Settings.Default.GENSHIN_IMPACT_DIR, HoyoGames.GenshinImpact, out bool _);
        ValidateSettings(AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR, HoyoGames.HonkaiStarRail, out bool _);
        ValidateSettings(AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR, HoyoGames.HonkaiImpactThird, out bool _);

        if (!AppSettings.Settings.Default.CHECKBOX_BACKGROUND)
            HoyoWindow.MediaElementBG.Source = null;

        if (AppSettings.Settings.Default.CHECKBOX_LASTGAME)
            LastGame();
    }

    public static void ValidateSettings(string GameConfigName, HoyoGames Game, out bool ErrorOccured)
    {
        ConfigRead GameConfigData = ConfigRead.GetConfig(GameConfigName);
        ErrorOccured = false;

        if (GameConfigData.FilePathNone)
            Game.GAME_DIR_VALID = false;
        else if (!GameConfigData.ConfigExist || Path.GetFileName(GameConfigData.GameStartName) != Game.GAME_EXECUTABLE)
            Game.GAME_DIR_VALID = !(ErrorOccured = true);
        else Game.GAME_DIR_VALID = true;
    }

    // Sets to the last game selected
    static void LastGame()
    {
        HoyoGames SelectedHoyoGame = null;
        short uid = AppSettings.Settings.Default.LAST_GAME;

        switch (uid)
        {
            case 1: SelectedHoyoGame = HoyoGames.GenshinImpact; break;
            case 2: SelectedHoyoGame = HoyoGames.HonkaiStarRail; break;
            case 3: SelectedHoyoGame = HoyoGames.HonkaiImpactThird; break;
        }

        if (SelectedHoyoGame is not null)
        {
            CurrentGameSelected = SelectedHoyoGame;
            GameChange.SetGame(--uid);
        }
    }

    static void UpdateConfig()
    {
        if (!AppSettings.Settings.Default.FIRSTRUN)
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

        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
    }

    private static void UpdateIsGameRunning()
    {
        if (CurrentGameSelected == HoyoGames.DEFAULT) return;

        HoyoWindow.HomeButton.IsEnabled = !IsGameRunning;

        HoyoWindow.LaunchButton.IsEnabled = false;
        HoyoWindow.LaunchButton.Content = LaunchText.GAME_LAUNCHED_TEXT;

        if (IsGameRunning) return;

        ConfigRead GameConfig = ConfigRead.GetConfig(CurrentGameSelected.GAME_DIRECTORY);

        string LaunchButtonContent = string.Empty switch
        {
            _ when CurrentGameSelected == HoyoGames.ZenlessZoneZero
                => LaunchText.GAME_SOON_TEXT,

            _ when !GameConfig.ConfigExist && CurrentGameSelected != HoyoGames.ZenlessZoneZero
                => LaunchText.GAME_NOTFOUND,

            _ => LaunchText.GAME_DEFAULT_TEXT
        };

        if (LaunchButtonContent == LaunchText.GAME_DEFAULT_TEXT)
            HoyoWindow.LaunchButton.IsEnabled = true;

        HoyoWindow.LaunchButton.Content = LaunchButtonContent;
    }
}