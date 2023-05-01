using System.Text.RegularExpressions;
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
    
    public static void GameChange(HoyoGames GameSelected)
    {
        Current.GameSelection.Visibility = Visibility.Hidden;

        if (Directory.Exists(GameSelected.DIR))
        {
            Current.MAIN_BACKGROUND.Children.Remove(Current.MainBG);
            Current.MAIN_BACKGROUND.Children.Remove(Current.HoyoTitleIMG);
            Current.LaunchButton.IsEnabled = true;

            Current.ChangeGame(Directory.GetFiles(GameSelected.DIR + "\\bg")[0]);

            foreach (string GameExecutable in Directory.GetFiles(GameSelected.DIR + "\\Games"))
                if (GameExecutable.Contains("exe") && !GameExecutable.Contains("Unity"))
                    ExecutableName = GameExecutable.ToString();

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