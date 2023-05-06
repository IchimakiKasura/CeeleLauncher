using static Define;
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
        HoyoGames HG = null;

        EventHandlers.Initialize();

        CheckGameDIRS(AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR, Current.GI_DEFAULT);
        CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, Current.HSR_DEFAULT);
        CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, Current.HI3_DEFAULT);

        var GameDirs = new List<(string dir, string name, string exec)>()
        {
            (AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR,  GENSHIN_IMPACT_TITLE,        GENSHIN_IMPACT_EXEC ),
            (AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, HONKAI_STAR_RAIL_TITLE,      HONKAI_STAR_RAIL_EXEC ),
            (AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, HONKAI_IMPACT_THIRD_TITLE,   HONKAI_IMPACT_THIRD_EXEC ),
            // (ZZZ_DIR_TXT.Text, "Zenless ZoneZero")
        };

        SettingWindow.Setting.ValidateGameFiles(GameDirs, out bool ErrorOccured, out string AppName);

        if(ErrorOccured)
            switch (AppName)
            {
                case "Genshin Impact": Current.GI_DEFAULT.IsEnabled = false; break;
                case "Honkai Star Rail": Current.HSR_DEFAULT.IsEnabled = false; break;
                case "Honkai Impact 3rd": Current.HI3_DEFAULT.IsEnabled = false; break;
            }


        switch(AppLocal.HoyoLauncher.Default.LAST_GAME)
        {
            case 1: HG = HoyoGames.GenshinImpact; break;
            case 2: HG = HoyoGames.HonkaiStarRail; break;
            case 3: HG = HoyoGames.HonkaiImpact3RD; break;
            case 4: HG = HoyoGames.ZenlessZoneZero; break;
        }

        if(HG is not null)
            GameChange(HG);
    }
    
    // TODO:
    // store Iniparser values on start and on setting change
    // to lessen File read every change of the game.
    public static void GameChange(HoyoGames GameSelected)
    {
        Current.GameSelection.Visibility = Visibility.Hidden;

        if (Directory.Exists(GameSelected.DIR))
        {
            SettingWindow.Setting.ValidateGameFiles(
                new List<(string,string,string)>
                {
                    (GameSelected.DIR, GameSelected.CURRENT_GAME, GameSelected.CURRENT_EXECUTABLE)
                },
                out bool ErrorOccured,
                out string AppName
            );

            if(ErrorOccured)
            {
                MessageBox.Show($"ERROR:\n\nThe \"{AppName}\" location cannot be found!\n or its an incorrect game.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var Config = GameConfig.Read(GameSelected.DIR);

            Current.HomeBG.Children.Remove(Current.MainBG);
            Current.HomeBG.Children.Remove(Current.HoyoTitleIMG);
            Current.CheckInPage.IsEnabled = true;
            Current.LaunchButton.IsEnabled = true;

            Current.ChangeGame(Path.Combine(Config.GameBackground));
            ExecutableName = Path.Combine(Config.GameFolder, Config.GameExecutable);
            
            CurrentGameSelected = GameSelected;
            AppLocal.HoyoLauncher.Default.Save();
            Current.LaunchButton.Content = GAME_DEFAULT_TEXT;
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