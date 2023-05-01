namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    private const string 
    GAME_LAUNCHED_TEXT = "Game is Running",
    GAME_DEFAULT_TEXT = "Launch";

    public static void Initialize()
    {
        Current.MinButton.Click += Minimize;
        Current.ExitButton.Click += Exit;
        Current.TopBorder.MouseDown += TopBarDrag;
        Current.LaunchSelection.Click += ThreeBarButton;
        Current.LaunchButton.Click += LaunchButton;

        Current.GAME_GI.Click += (s,e) => HoyoLauncher.GameChange(HoyoGames.GenshinImpact);
        Current.GAME_HSR.Click += (s,e) => HoyoLauncher.GameChange(HoyoGames.HonkaiStarRail);
        Current.GAME_HI3.Click += (s,e) => HoyoLauncher.GameChange(HoyoGames.HonkaiImpact3RD);
        Current.GAME_ZZZ.Click += (s,e) => HoyoLauncher.GameChange(HoyoGames.ZenlessZoneZero);

        Current.GI_DEFAULT.Click += (s,e) => HoyoLauncher.OpenOriginalLauncher(HoyoGames.GenshinImpact);
        Current.HSR_DEFAULT.Click += (s,e) => HoyoLauncher.OpenOriginalLauncher(HoyoGames.HonkaiStarRail);
        Current.HI3_DEFAULT.Click += (s,e) => HoyoLauncher.OpenOriginalLauncher(HoyoGames.HonkaiImpact3RD);
        // Current.ZZZ_DEFAULT.Click += (s,e) => HoyoLauncher.OpenOriginalLauncher(HoyoGames.ZenlessZoneZero);

        Current.GameHomePage.Click += (s,e) => Process.Start(new ProcessStartInfo() {
            FileName = HoyoLauncher.CurrentGameSelected.URL,
            UseShellExecute = true
        });

        Current.SettingsButton.Click += (s, e) => new SettingWindow.Setting { Owner = Current }.ShowDialog();
    }

    static void Exit(object sender, RoutedEventArgs e) => Current.Close();
    static void Minimize(object sender, RoutedEventArgs e) => Current.WindowState = WindowState.Minimized;
    static void ThreeBarButton(object sender, RoutedEventArgs e) =>
        Current.GameSelection.Visibility = Current.GameSelection.IsVisible ? Visibility.Hidden : Visibility.Visible;

    static void TopBarDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Left) Current.DragMove();
    }

    static async void LaunchButton(object s, RoutedEventArgs e)
    {
        e.Handled = true;

        Current.WindowState = WindowState.Minimized;

        HoyoLauncher.IsGameRunning = true;
        Current.LaunchButton.IsEnabled = false;
        Current.LaunchSelection.IsEnabled = false;

        // idk
        using var Proc = await Task.Run(()=>Process.Start(HoyoLauncher.ExecutableName));
        
        Current.LaunchButton.Content = GAME_LAUNCHED_TEXT;

        await Proc.WaitForExitAsync();

        HoyoLauncher.IsGameRunning = false;
        Current.LaunchButton.Content = GAME_DEFAULT_TEXT;
        Current.LaunchButton.IsEnabled = true;
        Current.LaunchSelection.IsEnabled = true;

        if(Current.WindowState is WindowState.Minimized)
            Current.WindowState = WindowState.Normal;
        
    }
}