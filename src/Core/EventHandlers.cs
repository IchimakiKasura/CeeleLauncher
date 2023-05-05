using static Define;
namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    public static void Initialize()
    {
        // Window buttons and others
        Current.MinButton.Click += Minimize;
        Current.ExitButton.Click += Exit;
        Current.TopBorder.MouseDown += TopBarDrag;
        Current.LaunchSelection.Click += ThreeBarButton;
        Current.LaunchButton.Click += LaunchButton;

        // Side Button's Tooltips
        Current.GI_DEFAULT.MouseEnter += SideButtonTooltips_Enter;
        Current.HSR_DEFAULT.MouseEnter += SideButtonTooltips_Enter;
        Current.HI3_DEFAULT.MouseEnter += SideButtonTooltips_Enter;
        Current.ZZZ_DEFAULT.MouseEnter += SideButtonTooltips_Enter;
        Current.TOT_DEFAULT.MouseEnter += SideButtonTooltips_Enter;
        Current.GameHomePage.MouseEnter += SideButtonTooltips_Enter;
        Current.CheckInPage.MouseEnter += SideButtonTooltips_Enter;

        Current.GI_DEFAULT.MouseLeave += SideButtonTooltips_Leave;
        Current.HSR_DEFAULT.MouseLeave += SideButtonTooltips_Leave;
        Current.HI3_DEFAULT.MouseLeave += SideButtonTooltips_Leave;
        Current.ZZZ_DEFAULT.MouseLeave += SideButtonTooltips_Leave;
        Current.TOT_DEFAULT.MouseLeave += SideButtonTooltips_Leave;
        Current.GameHomePage.MouseLeave += SideButtonTooltips_Leave;
        Current.CheckInPage.MouseLeave += SideButtonTooltips_Leave;

        Current.TOT_DEFAULT.Click += (s, e) =>
            Process.Start(new ProcessStartInfo() { FileName = HoyoGames.TearsOfThemis.DIR, UseShellExecute = true }).Dispose();

        // Launch / Play
        Current.GAME_GI.Click += ButtonSelectionClicked;
        Current.GAME_HSR.Click += ButtonSelectionClicked;
        Current.GAME_HI3.Click += ButtonSelectionClicked;

        // Original Launcher
        Current.GI_DEFAULT.Click += ButtonLauncherClicked;
        Current.HSR_DEFAULT.Click += ButtonLauncherClicked;
        Current.HI3_DEFAULT.Click += ButtonLauncherClicked;

        Current.GAME_ZZZ.Click += (s,e)=>
        {
            Current.GameSelection.Visibility = Visibility.Hidden;
            Current.TEMP_BACKGROUND.Children.Remove(Current.MainBG);
            Current.TEMP_BACKGROUND.Children.Remove(Current.HoyoTitleIMG);
            Current.CheckInPage.IsEnabled = false;
            Current.LaunchButton.IsEnabled = false;
            Current.LaunchButton.Content = "Coming Soon";
            Current.MAIN_BACKGROUND.Background = new ImageBrush(new BitmapImage(new(ZZZ_TEMP_BG, UriKind.RelativeOrAbsolute)));
            HoyoLauncher.CurrentGameSelected = HoyoGames.ZenlessZoneZero;
        };

        Current.ZZZ_DEFAULT.Click += (s,e) =>
            MessageBox.Show("Coming Soon!", "Zenless Zone Zero", MessageBoxButton.OK, MessageBoxImage.Information);

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

        using var Proc = Process.Start(new ProcessStartInfo() { FileName = HoyoLauncher.ExecutableName, UseShellExecute = true });
        
        Current.LaunchButton.Content = GAME_LAUNCHED_TEXT;

        await Proc.WaitForExitAsync();

        HoyoLauncher.IsGameRunning = false;
        Current.LaunchButton.Content = GAME_DEFAULT_TEXT;
        Current.LaunchButton.IsEnabled = true;
        Current.LaunchSelection.IsEnabled = true;

        if(Current.WindowState is WindowState.Minimized)
            Current.WindowState = WindowState.Normal;
        
    }

    static void SideButtonTooltips_Enter(object s, RoutedEventArgs e)
    {
        Current.ToolTipSideButton.Visibility = Visibility.Visible;
        short GameSelected = short.Parse(((System.Windows.Controls.Button)s).Uid);
        HoyoGames HG = null ;
        double BorderWidth = 0,
        CanvasLeft = 0,
        CanvasTop = 0;

        switch(GameSelected)
        {
            case 0:
                HG = HoyoGames.GenshinImpact;
                CanvasTop = 41;
                BorderWidth = 192;
                CanvasLeft = -88;
            break;

            case 1:
                HG = HoyoGames.HonkaiStarRail;
                CanvasTop = 118;
                BorderWidth = 196;
                CanvasLeft = -92;
            break;

            case 2:
                HG = HoyoGames.HonkaiImpact3RD;
                CanvasTop = 194;
                BorderWidth = 209;
                CanvasLeft = -105;
            break;
            case 3:
                HG = HoyoGames.ZenlessZoneZero;
                CanvasTop = 268;
                BorderWidth = 206;
                CanvasLeft = -102;
            break;
            case 4:
                HG = HoyoGames.TearsOfThemis;
                CanvasTop = 344;
                BorderWidth = 195;
                CanvasLeft = -91;
            break;
            case 5:
                CanvasTop = 476;
                BorderWidth = 120;
                CanvasLeft = -16;
            break;
            case 6:
                CanvasTop = 522;
                BorderWidth = 110;
                CanvasLeft = -6;
            break;
        }

        System.Windows.Controls.Canvas.SetLeft(Current.ToolTipSideButton_Border, CanvasLeft);
        System.Windows.Controls.Canvas.SetTop(Current.ToolTipSideButton, CanvasTop);
        Current.ToolTipSideButton_Border.Width = BorderWidth;

        if (HG is not null)
            Current.ToolTipSideButton_Text.Text = HG.TOOLTIPTEXT;
        else Current.ToolTipSideButton_Text.Text = GameSelected is 5 ? "Check-In Rewards" : "Game Hompage";
    }
    static void SideButtonTooltips_Leave(object s, RoutedEventArgs e) =>
        Current.ToolTipSideButton.Visibility = Visibility.Hidden;
    
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