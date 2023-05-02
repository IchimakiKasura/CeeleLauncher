namespace HoyoLauncherProject.Core;

sealed class HoyoLauncher
{
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
            var ConfigString = File.ReadAllText(Path.Combine(GameSelected.DIR,"config.ini"));
            var Data = new IniParser.IniDataParser().Parse(ConfigString);

            string
            GameFolder = Data["launcher"]["game_install_path"],
            GameExec = Data["launcher"]["game_start_name"],
            GameBG = Path.Combine(GameSelected.DIR, "bg", Data["launcher"]["game_dynamic_bg_name"]);

            Current.MAIN_BACKGROUND.Children.Remove(Current.MainBG);
            Current.MAIN_BACKGROUND.Children.Remove(Current.HoyoTitleIMG);
            Current.CheckInPage.IsEnabled = true;
            Current.LaunchButton.IsEnabled = true;

            Current.ChangeGame(Path.Combine(GameBG));
            ExecutableName = Path.Combine(GameFolder, GameExec);
            
            CurrentGameSelected = GameSelected;
        }
        else
            MessageBox.Show("No Directory found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static void OpenOriginalLauncher(HoyoGames args)
    {
        foreach(string Launcher in Directory.GetFiles(args.DIR))
            if(Regex.IsMatch(Launcher, @"(launcher|Launcher)" , RegexOptions.Compiled))
                Process.Start(Launcher);       
    }
    
    // for side buttons
    public static void CheckGameDIRS(string dir, System.Windows.Controls.Button btn) =>
        btn.IsEnabled = Directory.Exists(dir);
}