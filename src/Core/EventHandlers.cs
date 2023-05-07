namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    private static bool AlreadyMinimized = false;

    public static void Initialize()
    {
        // Window buttons and others
        Current.ExitButton.Click += (s,e) => Current.Close();
        Current.LaunchSelection.Click += (s,e) => Current.GameSelection.Visibility = Current.GameSelection.IsVisible ? Visibility.Hidden : Visibility.Visible;
        Current.MinButton.Click += Minimize;
        Current.TopBorder.MouseDown += TopBarDrag;
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

        // Launch / Play
        Current.GAME_GI.Click += ButtonSelectionClicked;
        Current.GAME_HSR.Click += ButtonSelectionClicked;
        Current.GAME_HI3.Click += ButtonSelectionClicked;
        Current.GAME_ZZZ.Click += (s, e) =>
        {
            Current.GameSelection.Visibility = Visibility.Hidden;
            Current.HomeBG.Children.Remove(Current.MainBG);
            Current.HomeBG.Children.Remove(Current.HoyoTitleIMG);
            Current.CheckInPage.IsEnabled = false;
            Current.LaunchButton.IsEnabled = false;
            Current.LaunchButton.Content = "Coming Soon";
            Current.MAIN_BACKGROUND.Background = new ImageBrush(new BitmapImage(new(ZZZ_TEMP_BG, UriKind.RelativeOrAbsolute)));
            HoyoLauncher.CurrentGameSelected = HoyoGames.ZenlessZoneZero;
        };

        // Original Launcher
        Current.GI_DEFAULT.Click += ButtonLauncherClicked;
        Current.HSR_DEFAULT.Click += ButtonLauncherClicked;
        Current.HI3_DEFAULT.Click += ButtonLauncherClicked;
        Current.ZZZ_DEFAULT.Click += (s,e) =>
            MessageBox.Show("Coming Soon!", "Zenless Zone Zero", MessageBoxButton.OK, MessageBoxImage.Information);
        Current.TOT_DEFAULT.Click += (s, e) =>
            Process.Start(new ProcessStartInfo() { FileName = HoyoGames.TearsOfThemis.DIR, UseShellExecute = true }).Dispose();

        Current.GameHomePage.Click += (s,e) => ExtraButtons(true);
        Current.CheckInPage.Click += (s,e) => ExtraButtons(false);

        Current.SettingsButton.Click += (s, e) => new SettingWindow.Setting { Owner = Current }.ShowDialog();
        Current.HomeButton.Click += delegate
        {
            if (Current.HomeBG.Children.Contains(Current.MainBG)) return;
            
            Current.MAIN_BACKGROUND.Background = null;
            Current.HomeBG.Children.Add(Current.MainBG);
            Current.HomeBG.Children.Add(Current.HoyoTitleIMG);
            Current.CheckInPage.IsEnabled = false;
            Current.LaunchButton.IsEnabled = false;
            Current.LaunchButton.Content = GAME_DEFAULT_TEXT;
            HoyoLauncher.CurrentGameSelected = HoyoGames.DEFAULT;
        };
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

    static void Minimize(object s, RoutedEventArgs e)
    {
        Current.WindowState = WindowState.Minimized;
        Current.ShowInTaskbar = false;
        Current.Hide();
        App.nIcon.Visible = true;

        if (!AlreadyMinimized)
        {
            App.nIcon.ShowBalloonTip(5);
            AlreadyMinimized = true;
        }
    }
    
    static void TopBarDrag(object s, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Left) Current.DragMove();
    }

    static async void LaunchButton(object s, RoutedEventArgs e)
    {
        e.Handled = true;

        Current.WindowState = WindowState.Minimized;
        Current.ShowInTaskbar = false;
        Current.Hide();
        App.nIcon.Visible = true;

        if (!AlreadyMinimized)
        {
            App.nIcon.ShowBalloonTip(5);
            AlreadyMinimized = true;
        }

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

        if(Current.WindowState is not WindowState.Minimized) return;
        Current.Show();
        Current.WindowState = WindowState.Normal;
        Current.ShowInTaskbar = true;
        App.nIcon.Visible = false;
    }

    static void SideButtonTooltips_Enter(object s, RoutedEventArgs e)
    {
        Current.ToolTipSideButton.Visibility = Visibility.Visible;
        short GameSelected = short.Parse(((Button)s).Uid);
        ToolTipPlacements placements = new();
        HoyoGames HG = null ;

        switch(GameSelected)
        {
            case 0:
                HG = HoyoGames.GenshinImpact;
                placements = new(41,-88,192);
            break;

            case 1:
                HG = HoyoGames.HonkaiStarRail;
                placements = new(118,-92,196);
            break;

            case 2:
                HG = HoyoGames.HonkaiImpact3RD;
                placements = new(194,-105,209);
            break;
            case 3:
                HG = HoyoGames.ZenlessZoneZero;
                placements = new(268,-102,206);
            break;
            case 4:
                HG = HoyoGames.TearsOfThemis;
                placements = new(344,-85,189);
            break;
            case 5: placements = new(476,-16,120); break;
            case 6: placements = new(522,-6,110);  break;
        }

        Canvas.SetLeft(Current.ToolTipSideButton_Border, placements.CanvasLeft);
        Canvas.SetTop(Current.ToolTipSideButton, placements.CanvasTop);
        Current.ToolTipSideButton_Border.Width = placements.BorderWidth;

        if (HG is not null)
            Current.ToolTipSideButton_Text.Text = HG.TOOLTIPTEXT;
        else Current.ToolTipSideButton_Text.Text = GameSelected is 5 ? "Check-In Rewards" : "Game Hompage";
    }
    static void SideButtonTooltips_Leave(object s, RoutedEventArgs e) =>
        Current.ToolTipSideButton.Visibility = Visibility.Hidden;
    
    public static void ButtonSelectionClicked(object s, RoutedEventArgs e)
    {
        short GameSelected = short.Parse(((Button)s).Uid);
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
        int GameSelected = int.Parse(((Button)s).Uid);
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