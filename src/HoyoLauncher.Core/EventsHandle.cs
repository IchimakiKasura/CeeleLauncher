namespace HoyoLauncher.Core;

[Events]
public sealed class EventsHandles
{
    public static void WindowHomeLogo() =>
        HoyoWindow.HoyoTitleIMG.Visibility = App.Config.CHECKBOX_TITLE ? Visibility.Visible : Visibility.Collapsed;
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
        HoyoWindow.GameOriginalLauncher.ButtonToolTip = ToolTips.LAUNCHER_TIP;
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
                case "MinButton": HoyoWindow.WindowState = WindowState.Minimized; break;
                case "SettingsButton": new HoyoLauncherSettings.HoyoSettings { Owner = HoyoWindow }.ShowDialog(); break;
                case "HomeButton":
                        new HoyoValues()
                        {
                            Background = HoyoGames.DEFAULT.GAME_DEFAULT_BG,
                            RemoveMainBG = false,
                            LaunchButton = false,
                            LaunchButtonContent = "Welcome",
                            VersionBubble = Visibility.Hidden
                        }
                        .ApplyChanges();

                        HoyoMain.CurrentGameSelected = HoyoGames.DEFAULT;
                        HoyoMain.RefreshSideButtons();
                        App.Config.LAST_GAME = 0;
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
                case "GameOriginalLauncher": Launcher = Path.Combine(HoyoMain.CurrentGameSelected.GAME_DIRECTORY, "launcher.exe"); break;
                case "GameScreenshotFolder": Launcher = Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, HoyoMain.CurrentGameSelected.GAME_SCREENSHOT_DIR); break;
                case "GameHomePage": Launcher = HoyoMain.CurrentGameSelected.GAME_HOMEPAGE; break;
                case "GameMapPage": Launcher = HoyoMain.CurrentGameSelected.GAME_MAP_PAGE; break;
                case "GENSHIN_IMPACT_REWARDS": Launcher = HoyoGames.GenshinImpact.GAME_CHECK_IN_PAGE; break;
                case "HONKAI_STAR_RAIL_REWARDS": Launcher = HoyoGames.HonkaiStarRail.GAME_CHECK_IN_PAGE; break;
                case "HONKAI_IMPACT_THIRD_REWARDS": Launcher = HoyoGames.HonkaiImpactThird.GAME_CHECK_IN_PAGE; break;
                case "TOT_SITE": Launcher = HoyoGames.TearsOfThemis.GAME_CHECK_IN_PAGE; break;
                case "ZZZ_REWARDS":
                    HoyoMessageBox.Show("Zenless Zone Zero", "Game is not released yet!", HoyoWindow);
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

            if(Equals(HoyoWindow.LaunchButton.Content, LaunchText.GAME_UPDATE_TEXT) || Equals(HoyoWindow.LaunchButton.Content, LaunchText.GAME_EXTRACT_TEXT))
            {
                if(File.Exists(Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(HoyoMain.CurrentGameSelected.API_CACHE.DownloadFile.LocalPath))))
                {
                    HoyoMain.ProcessStart(HoyoMain.ExecutableName);
                    HoyoMessageBox.Show("A Very Cool Message Box", "Opening Original Launcher to Update the game!\r\rIf the File is downloaded, You can just press the Update again on the Original Launcher\rAnd it will extract it smoothly.\r\r If the File was stopped mid-way of downloading, It will resume its progress on the Original Launcher.\r", HoyoWindow);
                }
                else
                {
                    HoyoWindow.HomeButton.IsEnabled =
                    HoyoWindow.LaunchSelection.IsEnabled =
                    HoyoWindow.LaunchButton.IsEnabled = false;
                    HoyoWindow.LaunchButton.Content = "Downloading";
                    RetrieveFile.DownloadFile(HoyoMain.CurrentGameSelected.API_CACHE.DownloadFile);
                }

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

            if(HoyoWindow.WindowState is WindowState.Minimized)
            {
                HoyoWindow.WindowState = WindowState.Normal;
                HoyoWindow.Show();
                HoyoWindow.ShowInTaskbar = true;
                App.AppTray.Visible = false;
            }

            HoyoWindow.Activate();
        };

    }

    public static void GameSelectionPopup()
    {
        static void PlayAnimation()
        {
            DoubleAnimation HeightAnimation = new(0,218, TimeSpan.FromMilliseconds(250))
            { EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut } };

            Storyboard.SetTargetProperty(HeightAnimation, new("Height"));
            Storyboard.SetTarget(HeightAnimation, HoyoWindow.GameSelection);
            Storyboard storyboard = new() { Children = new() { HeightAnimation } };
            storyboard.Completed += (s,e) => storyboard.Remove();
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

            HoyoMain.CurrentGameSelected = SelectedHoyoGame;
            HoyoMain.RefreshSideButtons();
            GameChange.SetGame(short.Parse(SelectedButton.Uid));
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

    public static void DownloadPauseButton()
    {
        Geometry Paused = Geometry.Parse("M8 6 L8 22 L13 22 L13 6 Z M17 6 L17 22 L22 22 L22 6 Z");
        Geometry Play = Geometry.Parse("M10 6 L10 22 L22 14 Z");

        HoyoWindow.ProgressBarButton.MouseDown += (s, e) =>
        {
            if (e.ChangedButton is not MouseButton.Left) return;

            RetrieveFile.IsPaused = !RetrieveFile.IsPaused;

            HoyoWindow.PathPause.Data = RetrieveFile.IsPaused ? Play : Paused;
        };

        HoyoWindow.ProgressBarButton.MouseEnter += (s, e) =>
            (e.Source as Border).Background = App.ConvertColorFromString("#9FFFFFFF");

        HoyoWindow.ProgressBarButton.MouseLeave += (s, e) =>
            (e.Source as Border).Background = App.ConvertColorFromString("#3FFFFFFF");
    }
    public static void PreDownloadBtn()
    {
        HoyoWindow.PreDownloadButton.MouseDown += (s,e) =>
        {
            if(e.ChangedButton is not MouseButton.Left) return;

            if(File.Exists(Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(HoyoMain.CurrentGameSelected.API_CACHE.PreDownloadFile.LocalPath))))
            {
                HoyoMessageBox.Show(HoyoWindow.Title, "The file is already downloaded!\r\rIf the download was stopped last time, It need to be re-downloaded again by deleting the file.\r", HoyoWindow);
                return;
            }

            HoyoWindow.HomeButton.IsEnabled =
            HoyoWindow.LaunchSelection.IsEnabled =
            HoyoWindow.LaunchButton.IsEnabled = false;
            HoyoWindow.LaunchButton.Content = "Downloading";
            HoyoWindow.PreDownload.Visibility = Visibility.Collapsed;
            RetrieveFile.DownloadFile(HoyoMain.CurrentGameSelected.API_CACHE.PreDownloadFile);
        };
    }
}