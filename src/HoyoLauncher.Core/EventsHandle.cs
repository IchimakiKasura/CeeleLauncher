namespace HoyoLauncher.Core;

[Events]
public sealed class EventsHandles
{
    public static void WindowHomeLogo() =>
        HoyoWindow.HoyoTitleIMG.Visibility = AppSettings.Settings.Default.CHECKBOX_TITLE ? Visibility.Visible : Visibility.Collapsed;
    public static void WindowSideButtonToolTips()
    {
        HoyoWindow.GENSHIN_IMPACT_REWARDS.ButtonToolTip = ToolTips.GENSHIN_IMPACT_TIP;
        HoyoWindow.HONKAI_STAR_RAIL_REWARDS.ButtonToolTip = ToolTips.HONKAI_STAR_RAIL_TIP;
        HoyoWindow.HONKAI_IMPACT_THIRD_REWARDS.ButtonToolTip = ToolTips.HONKAI_IMPACT_THIRD_TIP;
        HoyoWindow.ZZZ_REWARDS.ButtonToolTip = ToolTips.ZZZ_TIP;
        HoyoWindow.TOT_SITE.ButtonToolTip = ToolTips.TOT_TIP;
        HoyoWindow.GameHomePage.ButtonToolTip = ToolTips.HOMEPAGE_TIP;
        HoyoWindow.GameMapPage.ButtonToolTip = ToolTips.MAPPAGE_TIP;
        HoyoWindow.GameScreenshotFolder.ButtonToolTip = ToolTips.SCREENSHOT_TIP;
    }
    public static void WindowTopButtons()
    {
        List<Button> HoyoWindowButtons = new()
        {
            HoyoWindow.ExitButton,
            HoyoWindow.MinButton,
            HoyoWindow.SettingsButton,
            HoyoWindow.HomeButton
        };

        static void TopButtonClick(object sender, RoutedEventArgs events)
        {
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            var CurrentButton = (Button)sender;

            switch(CurrentButton.Name)
            {
                case "ExitButton": HoyoWindow.Close(); break;
                case "MinButton": App.AppMinimizeToTray(); break;
                case "SettingsButton": new HoyoLauncherSettings.HoyoSettings { Owner = HoyoWindow }.ShowDialog(); break;
                case "HomeButton":
                        if (HoyoWindow.HomeBG.Children.Contains(HoyoWindow.MainBG)) break;

                        new HoyoValues()
                        {
                            Background = null,
                            RemoveMainBG = false,
                            LaunchButton = false,
                            LaunchButtonContent = "Welcome",
                            VersionBubble = Visibility.Hidden
                        }
                        .ApplyChanges();

                        AppSettings.Settings.Default.LAST_GAME = 0;
                    break;
            }
        }

        foreach(Button button in HoyoWindowButtons)
            button.Click += TopButtonClick;

        HoyoWindow.TopBorder.MouseDown += App.DragMove<MainWindow>;
    }
    public static void WindowSideButtons()
    {
        static void GameLauncher(object s, RoutedEventArgs e)
        {
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            var CurrentButton = e.Source as HoyoButton;

            string Launcher = "";

            switch(CurrentButton.Name)
            {
                case "GameScreenshotFolder": Launcher = Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, HoyoMain.CurrentGameSelected.GAME_SCREENSHOT_DIR); break;
                case "GameHomePage": Launcher = HoyoMain.CurrentGameSelected.GAME_HOMEPAGE; break;
                case "GameMapPage": Launcher = HoyoMain.CurrentGameSelected.GAME_MAP_PAGE; break;
                case "GENSHIN_IMPACT_REWARDS": Launcher = HoyoGames.GenshinImpact.GAME_CHECK_IN_PAGE; break;
                case "HONKAI_STAR_RAIL_REWARDS": Launcher = HoyoGames.HonkaiStarRail.GAME_CHECK_IN_PAGE; break;
                case "HONKAI_IMPACT_THIRD_REWARDS": Launcher = HoyoGames.HonkaiImpactThird.GAME_CHECK_IN_PAGE; break;
                case "TOT_SITE": Launcher = HoyoGames.TearsOfThemis.GAME_CHECK_IN_PAGE; break;
                case "ZZZ_REWARDS":
                    MessageBox.Show("Game is not released yet!", "Zenless Zone Zero", MessageBoxButton.OK);
                    break;
            }

            if(Launcher is not "")
                HoyoMain.ProcessStart(Launcher);
        }

        HoyoWindow.SideButton_Click.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(GameLauncher));
    }
    public static void WindowLaunchGame()
    {
        HoyoWindow.LaunchButton.Click += async (s,e) =>
        {
            e.Handled = true;

            if(HoyoMain.CurrentGameSelected == HoyoGames.TearsOfThemis)
            {
                HoyoMain.ProcessStart(HoyoMain.CurrentGameSelected.GAME_DIRECTORY);
                return;
            }

            if(Equals(HoyoWindow.LaunchButton.Content, LaunchText.GAME_UPDATE_TEXT))
            {
                HoyoMain.ProcessStart(HoyoMain.ExecutableName);
                MessageBox.Show("Opening Original Launcher to Update the game!", "A Very Cool Message Box", MessageBoxButton.OK);
                return;
            }

            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;

            App.AppMinimizeToTray();
            HoyoMain.IsGameRunning = true;

            using Process GameProcess = Process.Start(
                new ProcessStartInfo()
                {
                    FileName = HoyoMain.ExecutableName,
                    UseShellExecute = true
                }
            );

            await GameProcess.WaitForExitAsync();
            HoyoMain.IsGameRunning = false;

            if(HoyoWindow.WindowState is not WindowState.Minimized) return;

            HoyoWindow.Show();
            HoyoWindow.WindowState = WindowState.Normal;
            HoyoWindow.ShowInTaskbar = true;
            App.AppTray.Visible = false;
        };

    }

    public static void GameSelectionPopup()
    {
        void PlayAnimation()
        {
            Storyboard storyboard = new();
            ThicknessAnimation MarginAnimation = new(
                new(925, 580, 128, 120),
                new(925, 380, 128, 120),
                TimeSpan.FromMilliseconds(250)
            );

            Storyboard.SetTargetProperty(MarginAnimation, new("Margin"));
            Storyboard.SetTarget(MarginAnimation, HoyoWindow.GameSelection);
            storyboard.Children.Add(MarginAnimation);
            storyboard.Begin();
        }

        HoyoWindow.LaunchSelection.Click += (s, e) =>
        {
            HoyoWindow.GameSelection.Visibility =
            HoyoWindow.GameSelection.IsVisible ? Visibility.Collapsed : Visibility.Visible;

            if (HoyoWindow.GameSelection.Visibility is Visibility.Visible)
                PlayAnimation();
        };
    }
    public static void GameSelectionButtonClick()
    {
        static void AutoClose(object s, RoutedEventArgs e)
        {
            var SelectedButton = (Button)e.Source;
            HoyoWindow.GameSelection.Visibility = Visibility.Collapsed;
            HoyoGames SelectedHoyoGame = null;

            switch(SelectedButton.Name)
            {
                case "GAME_SELECTION_GI" : SelectedHoyoGame = HoyoGames.GenshinImpact;        break;
                case "GAME_SELECTION_HSR": SelectedHoyoGame = HoyoGames.HonkaiStarRail;       break;
                case "GAME_SELECTION_HI3": SelectedHoyoGame = HoyoGames.HonkaiImpactThird;    break;
                case "GAME_SELECTION_ZZZ": SelectedHoyoGame = HoyoGames.ZenlessZoneZero;      break;
                case "GAME_SELECTION_TOT": SelectedHoyoGame = HoyoGames.TearsOfThemis;        break;
            }

            HoyoWindow.GameMapPage.IsEnabled = SelectedHoyoGame.GAME_MAP_PAGE is not "";
            HoyoWindow.GameScreenshotFolder.IsEnabled = SelectedHoyoGame.GAME_SCREENSHOT_DIR is not "" && SelectedHoyoGame.GAME_DIR_VALID;

            HoyoMain.CurrentGameSelected = SelectedHoyoGame;
            GameChange.SetGame(SelectedButton.Uid);
        }

        HoyoWindow.GameSelection_Click.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(AutoClose));
    }
    public static void GameSelectionBackgroundSet()
    {
        HoyoWindow.GAME_SELECTION_GI.Background = SelectionResources.GenshinImage;
        HoyoWindow.GAME_SELECTION_HSR.Background = SelectionResources.HonkaiStarRailImage;
        HoyoWindow.GAME_SELECTION_HI3.Background = SelectionResources.HonkaiImpactImage;
        HoyoWindow.GAME_SELECTION_ZZZ.Background = SelectionResources.ZZZImage;
        HoyoWindow.GAME_SELECTION_TOT.Background = SelectionResources.TOTImage;
    }
}