namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    private const string 
    GAME_LAUNCHED_TEXT = "Game is Running",
    GAME_DEFAULT_TEXT = "Launch";

    private static string ExecutableName;

    public static void Initialize()
    {
        Current.MinButton.Click += Minimize;
        Current.ExitButton.Click += Exit;
        Current.TopBorder.MouseDown += TopBarDrag;
        Current.LaunchSelection.Click += ThreeBarButton;

        Current.GAME_GI.Click += (s,e) => GameChange(HoyoGames.GenshinImpact);
        Current.GAME_HSR.Click += (s,e) => GameChange(HoyoGames.HonkaiStarRail);
        Current.GAME_HI3.Click += (s,e) => GameChange(HoyoGames.HonkaiImpact3RD);
        Current.GAME_ZZZ.Click += (s,e) => GameChange(HoyoGames.ZenlessZoneZero);

        Current.LaunchButton.Click += async (s,e) =>
        {
            e.Handled = true;

            Current.WindowState = WindowState.Minimized;
            var Proc = System.Diagnostics.Process.Start(ExecutableName);

            Current.LaunchButton.Content = GAME_LAUNCHED_TEXT;
            Current.LaunchButton.IsEnabled = false;
            Current.LaunchSelection.IsEnabled = false;
            HoyoLauncher.IsGameRunning = true;

            await Proc.WaitForExitAsync();
            Current.LaunchButton.Content = GAME_DEFAULT_TEXT;
            Current.LaunchButton.IsEnabled = true;
            Current.LaunchSelection.IsEnabled = true;
            HoyoLauncher.IsGameRunning = false;
        };

        Current.SettingsButton.Click += (s, e) => new SettingWindow.Setting().ShowDialog();
    }

    static void Exit(object sender, RoutedEventArgs e) => Current.Close();
    static void Minimize(object sender, RoutedEventArgs e) => Current.WindowState = WindowState.Minimized;
    static void ThreeBarButton(object sender, RoutedEventArgs e) =>
        Current.GameSelection.Visibility = Current.GameSelection.IsVisible ?
            Visibility.Hidden : Visibility.Visible;

    static void TopBarDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Left) Current.DragMove();
    }

    static void GameChange(string GameSelected)
    {
        Current.GameSelection.Visibility = Visibility.Hidden;

        if(Directory.Exists(GameSelected))
        {
            Current.MAIN_BACKGROUND.Children.Remove(Current.MainBG);
            Current.MAIN_BACKGROUND.Children.Remove(Current.HoyoTitleIMG);
            Current.LaunchButton.IsEnabled = true;

            Current.ChangeGame(Directory.GetFiles(GameSelected + "\\bg")[0]);

            foreach (string GameExecutable in Directory.GetFiles(GameSelected + "\\Games"))
                if(GameExecutable.Contains("exe") && !GameExecutable.Contains("Unity"))
                    ExecutableName = GameExecutable.ToString();
        }
        else
            MessageBox.Show("No Directory found!","Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}