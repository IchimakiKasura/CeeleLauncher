namespace HoyoLauncherProject.Core;

sealed class HoyoLauncher
{
    public static bool IsGameRunning { get; set; } = false;
    public static string ExecutableName { get; set; }

    public static void Initialize()
    {
        EventHandlers.Initialize();
    }
    public static void GameChange(string GameSelected)
    {
        Current.GameSelection.Visibility = Visibility.Hidden;

        if (Directory.Exists(GameSelected))
        {
            Current.MAIN_BACKGROUND.Children.Remove(Current.MainBG);
            Current.MAIN_BACKGROUND.Children.Remove(Current.HoyoTitleIMG);
            Current.LaunchButton.IsEnabled = true;

            Current.ChangeGame(Directory.GetFiles(GameSelected + "\\bg")[0]);

            foreach (string GameExecutable in Directory.GetFiles(GameSelected + "\\Games"))
                if (GameExecutable.Contains("exe") && !GameExecutable.Contains("Unity"))
                    ExecutableName = GameExecutable.ToString();
        }
        else
            MessageBox.Show("No Directory found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}