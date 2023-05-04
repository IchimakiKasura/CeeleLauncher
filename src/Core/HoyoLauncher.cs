namespace HoyoLauncherProject.Core;

sealed partial class HoyoLauncher
{
    [GeneratedRegex("(launcher|Launcher)", RegexOptions.Compiled)]
    private static partial Regex LauncherName();
    
    public static bool IsGameRunning { get; set; }
    public static string ExecutableName { get; set; }
    public static HoyoGames CurrentGameSelected { get; set; } = HoyoGames.DEFAULT;

    public static void Initialize()
    {
        IsGameRunning = false;

        EventHandlers.Initialize();

        CheckGameDIRS(AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR, Current.GI_DEFAULT);
        CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, Current.HSR_DEFAULT);
        CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, Current.HI3_DEFAULT);
    }
    
    // TODO:
    // store Iniparser values on start and on setting change
    // to lessen File read every change of the game.
    public static void GameChange(HoyoGames GameSelected)
    {
        Current.GameSelection.Visibility = Visibility.Hidden;

        if (Directory.Exists(GameSelected.DIR))
        {
            var Config = GameConfig.Read(GameSelected.DIR);

            Current.MAIN_BACKGROUND.Children.Remove(Current.MainBG);
            Current.MAIN_BACKGROUND.Children.Remove(Current.HoyoTitleIMG);
            Current.CheckInPage.IsEnabled = true;
            Current.LaunchButton.IsEnabled = true;

            Current.ChangeGame(Path.Combine(Config.GameBackground));
            ExecutableName = Path.Combine(Config.GameFolder, Config.GameExecutable);
            
            CurrentGameSelected = GameSelected;
        }
        else
            MessageBox.Show("No Directory found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static void OpenOriginalLauncher(HoyoGames args)
    {
        foreach(string Launcher in Directory.GetFiles(args.DIR))
            if(LauncherName().IsMatch(Launcher))
                Process.Start(Launcher);       
    }
    
    // for side buttons
    public static void CheckGameDIRS(string dir, System.Windows.Controls.Button btn) =>
        btn.IsEnabled = Directory.Exists(dir);

}