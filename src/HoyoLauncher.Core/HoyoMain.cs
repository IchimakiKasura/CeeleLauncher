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

    public static async void Initialize()
    {
        if (!MainConfig.IsConfigExist)
            MainConfig.CreateConfig();

        App.Config = await MainConfig.ReadConfig();

        if(App.Config.CUSTOM_BACKGROUND is not "" or null)
            HoyoWindow.MainBG.Background = DefaultBG.DEFAULT = new(new BitmapImage(new(App.Config.CUSTOM_BACKGROUND)))
            {
                Stretch=Stretch.UniformToFill
            };

        AppFirstRun();

        EventsAttribute.SetEvents();

        ValidateSettings(App.Config.GI_DIR, HoyoGames.GenshinImpact);
        ValidateSettings(App.Config.HSR_DIR, HoyoGames.HonkaiStarRail);
        ValidateSettings(App.Config.HI3_DIR, HoyoGames.HonkaiImpactThird);

        if (!App.Config.CHECKBOX_BACKGROUND)
            HoyoWindow.MediaElementBG.Source = null;

        if (App.Config.CHECKBOX_LAST_GAME)
            LastGame();

        if(CurrentGameSelected == HoyoGames.DEFAULT)
            RefreshSideButtons();

        HoyoWindow.Height *= App.Config.SCALING;
        HoyoWindow.Width *= App.Config.SCALING;
    }

    public static void ValidateSettings(string GameConfigName, HoyoGames Game) =>
        ValidateSettings(GameConfigName, Game, out bool _);
    public static void ValidateSettings(string GameConfigName, HoyoGames Game, out bool ErrorOccured)
    {
        GameConfigRead GameConfigData = Game.GAME_CONFIG_CACHE = GameConfigRead.GetConfig(GameConfigName);
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
        short uid = App.Config.LAST_GAME;

        switch (uid)
        {
            case 1: SelectedHoyoGame = HoyoGames.GenshinImpact; break;
            case 2: SelectedHoyoGame = HoyoGames.HonkaiStarRail; break;
            case 3: SelectedHoyoGame = HoyoGames.HonkaiImpactThird; break;
        }

        if (SelectedHoyoGame is null) return;

        new HoyoValues()
        {
            RemoveMainBG = true,
            Background = SelectedHoyoGame.GAME_DEFAULT_BG,
            LaunchButtonContent = "Loading",
            VersionBubble = Visibility.Collapsed,
            PreInstall = Visibility.Collapsed,
        }.ApplyChanges();

        CurrentGameSelected = SelectedHoyoGame;

        RefreshSideButtons();
        GameChange.SetGame(--uid);
    }

    static void AppFirstRun()
    {
        if (App.Config.FIRST_RUN) return;
        
        HoyoWindow.Loaded += async (s, e) =>
        {
            await Task.Delay(1000);
            new ShortTour { Owner = HoyoWindow }.ShowDialog();
        };

        App.Config.FIRST_RUN = true;
    }

    // boilerplate
    public static void ProcessStart(string FileName)
    {
        using Process process = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                Domain = "explorer.exe",
                FileName = FileName,
                UseShellExecute = true
            }
        };
        process.Start();
    }

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

    public static void RefreshSideButtons()
    {
        HoyoWindow.GameOriginalLauncher.IsEnabled =
        HoyoWindow.GameMapPage.IsEnabled =
        HoyoWindow.GameScreenshotFolder.IsEnabled = false;

        if(CurrentGameSelected == HoyoGames.DEFAULT) return;

        HoyoWindow.GameMapPage.IsEnabled = CurrentGameSelected.GAME_MAP_PAGE is not "";
        HoyoWindow.GameOriginalLauncher.IsEnabled = CurrentGameSelected.GAME_DIRECTORY is not "" && CurrentGameSelected.GAME_DIR_VALID;
        HoyoWindow.GameScreenshotFolder.IsEnabled = CurrentGameSelected.GAME_SCREENSHOT_DIR is not "" && CurrentGameSelected.GAME_DIR_VALID;
    }

    private static void UpdateIsGameRunning()
    {
        if (CurrentGameSelected == HoyoGames.DEFAULT) return;

        HoyoWindow.HomeButton.IsEnabled = !IsGameRunning;

        HoyoWindow.LaunchButton.IsEnabled = false;
        HoyoWindow.LaunchButton.Content = LaunchText.GAME_LAUNCHED_TEXT;

        if (IsGameRunning) return;

        GameConfigRead GameConfig = CurrentGameSelected.GAME_CONFIG_CACHE;

        string LaunchButtonContent = ContentSwitch(GameConfig);

        if (LaunchButtonContent == LaunchText.GAME_DEFAULT_TEXT)
            HoyoWindow.LaunchButton.IsEnabled = true;

        HoyoWindow.LaunchButton.Content = LaunchButtonContent;
    }

    private static string ContentSwitch(GameConfigRead GameConfig)
    {
        string DefaultText = LaunchText.GAME_DEFAULT_TEXT;

        if(!GameConfig.ConfigExist)
            DefaultText = LaunchText.GAME_NOTFOUND;

        if(CurrentGameSelected == HoyoGames.ZenlessZoneZero)
            DefaultText = LaunchText.GAME_SOON_TEXT;

        return DefaultText;
    }
}