using static define;
namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    public static void Initialize()
    {
        Current.MinButton.Click += Minimize;
        Current.ExitButton.Click += Exit;
        Current.TopBorder.MouseDown += TopBarDrag;
        Current.LaunchSelection.Click += ThreeBarButton;
        Current.LaunchButton.Click += LaunchButton;

        Current.GAME_GI.Click += ButtonSelectionClicked;
        Current.GAME_HSR.Click += ButtonSelectionClicked;
        Current.GAME_HI3.Click += ButtonSelectionClicked;
        Current.GAME_ZZZ.Click += ButtonSelectionClicked;

        Current.GI_DEFAULT.Click += ButtonLauncherClicked;
        Current.HSR_DEFAULT.Click += ButtonLauncherClicked;
        Current.HI3_DEFAULT.Click += ButtonLauncherClicked;

        Current.GameHomePage.Click += (s,e) => ExtraButtons(true);
        Current.CheckInPage.Click += (s,e) => ExtraButtons(false);

        Current.SettingsButton.Click += (s, e) => new SettingWindow.Setting { Owner = Current }.ShowDialog();
    }

    static void ExtraButtons(bool IsHomePage)
    {
        string filename = IsHomePage ? HoyoLauncher.CurrentGameSelected.URL : HoyoLauncher.CurrentGameSelected.CHECKIN_URL;
        var PSI = new ProcessStartInfo() {
            FileName = filename,
            UseShellExecute = true
        };
        Process.Start(PSI).Dispose();
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
        Current.GameSelection.Visibility = Visibility.Hidden;

        HoyoLauncher.IsGameRunning = true;
        Current.LaunchButton.IsEnabled = false;
        Current.LaunchSelection.IsEnabled = false;

        using var Proc = Process.Start(HoyoLauncher.ExecutableName);
        
        Current.LaunchButton.Content = GAME_LAUNCHED_TEXT;

        await Proc.WaitForExitAsync();

        HoyoLauncher.IsGameRunning = false;
        Current.LaunchButton.Content = GAME_DEFAULT_TEXT;
        Current.LaunchButton.IsEnabled = true;
        Current.LaunchSelection.IsEnabled = true;

        if(Current.WindowState is WindowState.Minimized)
            Current.WindowState = WindowState.Normal;
        
    }

    public static void ButtonSelectionClicked(object s, RoutedEventArgs e)
    {
        short GameSelected = short.Parse(((System.Windows.Controls.Button)s).Uid);
        HoyoGames HG = null;

        switch(GameSelected)
        {
            case 0: HG = HoyoGames.GenshinImpact; break;
            case 1: HG = HoyoGames.HonkaiStarRail; break;
            case 2: HG = HoyoGames.HonkaiImpact3RD; break;
            case 3: HG = HoyoGames.ZenlessZoneZero; break;
        }

        AppLocal.HoyoLauncher.Default.LAST_GAME = GameSelected += 1;
        HoyoLauncher.GameChange(HG);
    }

    public static void ButtonLauncherClicked(object s, RoutedEventArgs e)
    {
        int GameSelected = int.Parse(((System.Windows.Controls.Button)s).Uid);
        HoyoGames HG = null;

        switch(GameSelected)
        {
            case 0: HG = HoyoGames.GenshinImpact; break;
            case 1: HG = HoyoGames.HonkaiStarRail; break;
            case 2: HG = HoyoGames.HonkaiImpact3RD; break;
        }

        HoyoLauncher.OpenOriginalLauncher(HG);
    }
}