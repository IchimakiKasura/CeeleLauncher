namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    public static void Initialize()
    {
        current.ExitButton.Click += Exit;
        current.MinButton.Click += Minimize;
        current.TopBorder.MouseDown += TopBarDrag;
        current.LaunchSelection.Click += ThreeBarButton;

        current.GAME_GI.Click += (s, e) => GameChange(HoyoGames.GenshinImpact);
        current.GAME_HSR.Click += (s, e) => GameChange(HoyoGames.HonkaiStarRail);
        current.GAME_HI3.Click += (s, e) => GameChange(HoyoGames.HonkaiImpact3RD);
        current.GAME_ZZZ.Click += (s, e) => GameChange(HoyoGames.ZenlessZoneZero);

        current.LaunchButton.Click += (s,e) =>
        {
            current.WindowState = WindowState.Minimized;
        };
    }

    static void Exit(object sender, RoutedEventArgs e) => current.Close();
    static void Minimize(object sender, RoutedEventArgs e) => current.WindowState = WindowState.Minimized;
    static void TopBarDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Left) current.DragMove();
    }
    static void ThreeBarButton(object sender, RoutedEventArgs e)
    {
        current.GameSelection.Visibility = current.GameSelection.IsVisible ?
            Visibility.Hidden : Visibility.Visible;
    }

    static void GameChange(HoyoGames HG)
    {
        current.GameSelection.Visibility = Visibility.Hidden;

        string GameSelected = "" switch
        {
            _ when HG is HoyoGames.GenshinImpact =>
                Settings.HoyoLauncher.Default.GENSHIN_IMPACT_DIR,
            _ when HG is HoyoGames.HonkaiStarRail =>
                Settings.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR,
            _ when HG is HoyoGames.HonkaiImpact3RD =>
                Settings.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR,
            _ when HG is HoyoGames.ZenlessZoneZero =>
                Settings.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR,
            _ => null 
        };

        if(Directory.Exists(GameSelected))
        {
            current.MAIN_BACKGROUND.Children.Remove(current.MainBG);
            current.MAIN_BACKGROUND.Children.Remove(current.HoyoTitleIMG);
            current.LaunchButton.IsEnabled = true;
            current.ChangeGame(Directory.GetFiles(GameSelected + "\\bg")[0]);
        }
        else
            MessageBox.Show("No Directory found!","Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}