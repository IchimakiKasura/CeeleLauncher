using AppRes = AppResources.Resources;
namespace HoyoLauncher.HoyoLauncher_Core;

[Events]
public sealed class EventsHandles
{
    public static void WindowBackground() =>
        HoyoWindow.MediaElementBG.Source = App.TempBG;
    public static void WindowSideButtonToolTips()
    {
        HoyoWindow.GENSHIN_IMPACT_LAUNCHER.ButtonToolTip = AppRes.GENSHIN_IMPACT_TIP;
        HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER.ButtonToolTip = AppRes.HONKAI_STAR_RAIL_TIP;
        HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER.ButtonToolTip = AppRes.HONKAI_IMPACT_THIRD_TIP;
        HoyoWindow.ZZZ_LAUNCHER.ButtonToolTip = AppRes.ZZZ_TIP;
        HoyoWindow.TOT_LAUNCHER.ButtonToolTip = AppRes.TOT_TIP;
        HoyoWindow.CheckInPage.ButtonToolTip = AppRes.CHECKIN_TIP;
        HoyoWindow.GameHomePage.ButtonToolTip = AppRes.HOMEPAGE_TIP;
    }
    public static void WindowTopButtons()
    {
        static void TopButtonClick(object sender, RoutedEventArgs e)
        {
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            var CurrentButton = (Button)sender;

            switch(CurrentButton.Name)
            {
                case "ExitButton":
                        HoyoWindow.Close();
                    break;

                case "MinButton":
                        App.AppMinimizeToTray();
                    break;

                case "SettingsButton":
                        new HoyoLauncherSettings.HoyoSettings { Owner = HoyoWindow }.ShowDialog();
                    break;

                case "HomeButton":
                        if (HoyoWindow.HomeBG.Children.Contains(HoyoWindow.MainBG)) break;

                        HoyoChange.SetValues(
                            new HoyoValues(
                                null,
                                false,
                                false,
                                false,
                                AppRes.GAME_DEFAULT_TEXT
                            )
                        );

                        AppSettings.Settings.Default.LAST_GAME = 0;
                        AppSettings.Settings.Default.Save();
                    break;
            }
        }

        HoyoWindow.ExitButton.Click += TopButtonClick;
        HoyoWindow.MinButton.Click += TopButtonClick;
        HoyoWindow.SettingsButton.Click += TopButtonClick;
        HoyoWindow.HomeButton.Click += TopButtonClick;
        HoyoWindow.TopBorder.MouseDown += (s, e) => { if (e.ChangedButton is MouseButton.Left) HoyoWindow.DragMove(); };
    }
    public static void WindowSideButtons()
    {
        static void GameLauncher(object s, RoutedEventArgs e)
        {
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            // implicit or explicit to System Button wont fucking work
            // because the "s" which is an object currently; is a non-instance
            // so its throwing the "InvalidCastException"
            var CurrentButton = s as HoyoButton;
            string Launcher = "";

            switch(CurrentButton.Name)
            {
                case "GENSHIN_IMPACT_LAUNCHER":
                    Launcher = Path.Combine(HoyoGames.GenshinImpact.GAME_DIRECTORY, "launcher.exe");
                    break;
                
                case "HONKAI_STAR_RAIL_LAUNCHER":
                    Launcher = Path.Combine(HoyoGames.HonkaiStarRail.GAME_DIRECTORY, "launcher.exe");
                    break;
                
                case "HONKAI_IMPACT_THIRD_LAUNCHER":
                    Launcher = Path.Combine(HoyoGames.HonkaiImpactThird.GAME_DIRECTORY, "launcher.exe");
                    break;
                
                case "ZZZ_LAUNCHER":
                    MessageBox.Show("Game is not released yet!", "Zenless Zone Zero", MessageBoxButton.OK);
                    break;
                
                case "TOT_LAUNCHER":
                    Launcher = HoyoGames.TearsOfThemis.GAME_DIRECTORY;
                    break;
            }

            if(Launcher is not "")
                HoyoMain.ProcessStart(Launcher);
        }

        HoyoWindow.GENSHIN_IMPACT_LAUNCHER.Click += GameLauncher;
        HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER.Click += GameLauncher;
        HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER.Click += GameLauncher;
        HoyoWindow.ZZZ_LAUNCHER.Click += GameLauncher;
        HoyoWindow.TOT_LAUNCHER.Click += GameLauncher;

        HoyoWindow.GameHomePage.Click += (s,e) =>
            HoyoMain.ProcessStart(HoyoMain.CurrentGameSelected.GAME_HOMEPAGE);

        HoyoWindow.CheckInPage.Click += (s,e) =>
            HoyoMain.ProcessStart(HoyoMain.CurrentGameSelected.GAME_CHECK_IN_PAGE);
    }
    public static void WindowLaunchGame()
    {
        HoyoWindow.LaunchButton.Click += async (s,e) =>
        {
            e.Handled = true;
            var MainButton = (Button)s;

            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;

            // Window Hide
            App.AppMinimizeToTray();
            
            // Buttons
            MainButton.IsEnabled =
            HoyoWindow.HomeButton.IsEnabled = 
            HoyoWindow.LaunchSelection.IsEnabled = false;
            MainButton.Content = AppRes.GAME_LAUNCHED_TEXT;

            HoyoMain.IsGameRunning = true;

            using var GameProcess = Process.Start(
                new ProcessStartInfo()
                {
                    FileName = HoyoMain.ExecutableName,
                    UseShellExecute = true
                }
            );

            await GameProcess.WaitForExitAsync();

            HoyoMain.IsGameRunning = false;
            MainButton.Content = AppRes.GAME_DEFAULT_TEXT;
            MainButton.IsEnabled =
            HoyoWindow.HomeButton.IsEnabled = 
            HoyoWindow.LaunchSelection.IsEnabled = true;

            if(HoyoWindow.WindowState is not WindowState.Minimized) return;

            HoyoWindow.Show();
            HoyoWindow.WindowState = WindowState.Normal;
            HoyoWindow.ShowInTaskbar = true;
            App.AppTray.Visible = false;
        };
    }

    public static void GameSelectionPopup() =>
        HoyoWindow.LaunchSelection.Click += (s, e) =>
            HoyoWindow.GameSelection.Visibility =
                HoyoWindow.GameSelection.IsVisible ? Visibility.Hidden : Visibility.Visible;
    public static void GameSelectionButtonClick()
    {
        static void AutoClose(object s, RoutedEventArgs e)
        {
            var SelectedButton = (Button)s;
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            HoyoGames HG = null;

            switch(SelectedButton.Name)
            {
                case "GAME_SELECTION_GI" : HG = HoyoGames.GenshinImpact;        break;
                case "GAME_SELECTION_HSR": HG = HoyoGames.HonkaiStarRail;       break;
                case "GAME_SELECTION_HI3": HG = HoyoGames.HonkaiImpactThird;    break;
                case "GAME_SELECTION_ZZZ": HG = HoyoGames.ZenlessZoneZero;      break;
            }

            HoyoMain.GameChange(HG, short.Parse(SelectedButton.Uid));
        }

        HoyoWindow.GAME_SELECTION_GI.Click += AutoClose;
        HoyoWindow.GAME_SELECTION_HSR.Click += AutoClose;
        HoyoWindow.GAME_SELECTION_HI3.Click += AutoClose;
        HoyoWindow.GAME_SELECTION_ZZZ.Click += AutoClose;
    }
    public static void GameSelectionBackgroundSet()
    {
        HoyoWindow.GAME_SELECTION_GI.Background = SelectionResources.GenshinImage;
        HoyoWindow.GAME_SELECTION_HSR.Background = SelectionResources.HonkaiStarRailImage;
        HoyoWindow.GAME_SELECTION_HI3.Background = SelectionResources.HonkaiImpactImage;
        HoyoWindow.GAME_SELECTION_ZZZ.Background = SelectionResources.ZZZImage;
    }
}