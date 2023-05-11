﻿namespace HoyoLauncher.HoyoLauncher_Core;

public sealed class EventsHandle
{
    public static void WindowTransparency() =>
        HoyoWindow.Loaded += (s, e) =>
            new WindowTransparency(HoyoWindow).MakeTransparent();
    public static void WindowBackground() =>
        HoyoWindow.MediaElementBG.Source = new(App.TempBG);
    public static void WindowSideButtonToolTips()
    {
        HoyoWindow.GENSHIN_IMPACT_LAUNCHER.ButtonToolTip = AppResources.Resources.GENSHIN_IMPACT_TIP;
        HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER.ButtonToolTip = AppResources.Resources.HONKAI_STAR_RAIL_TIP;
        HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER.ButtonToolTip = AppResources.Resources.HONKAI_IMPACT_THIRD_TIP;
        HoyoWindow.ZZZ_LAUNCHER.ButtonToolTip = AppResources.Resources.ZZZ_TIP;
        HoyoWindow.TOT_LAUNCHER.ButtonToolTip = AppResources.Resources.TOT_TIP;
        HoyoWindow.CheckInPage.ButtonToolTip = AppResources.Resources.CHECKIN_TIP;
        HoyoWindow.GameHomePage.ButtonToolTip = AppResources.Resources.HOMEPAGE_TIP;
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
                        HoyoWindow.WindowState = WindowState.Minimized;
                        HoyoWindow.ShowInTaskbar = false;
                        HoyoWindow.Hide();
                        App.AppTray.Visible = true;

                        if(HoyoMain.FirstRun)
                        {
                            App.AppTray.ShowBalloonTip(3);
                            HoyoMain.FirstRun = false;
                        }
                    break;

                case "SettingsButton":
                        var WindowSetting = new HoyoLauncherSettings.HoyoSettings { Owner = HoyoWindow }.ShowDialog();
                    break;

                case "HomeButton":
                        if (HoyoWindow.HomeBG.Children.Contains(HoyoWindow.MainBG)) break;

                        HoyoWindow.WINDOW_BORDER.Background = null;
                        HoyoWindow.HomeBG.Children.Add(HoyoWindow.MainBG);
                        HoyoWindow.HomeBG.Children.Add(HoyoWindow.HoyoTitleIMG);
                        HoyoWindow.CheckInPage.IsEnabled = false;
                        HoyoWindow.LaunchButton.IsEnabled = false;
                        HoyoWindow.LaunchButton.Content = AppResources.Resources.GAME_DEFAULT_TEXT;

                        AppSettings.Settings.Default.LAST_GAME = 0;
                        AppSettings.Settings.Default.Save();
                    break;
            }
        }

        HoyoWindow.ExitButton.Click += TopButtonClick;
        HoyoWindow.MinButton.Click += TopButtonClick;
        HoyoWindow.SettingsButton.Click += TopButtonClick;
        HoyoWindow.HomeButton.Click += TopButtonClick;
        HoyoWindow.TopBorder.MouseDown += (s, e) => HoyoWindow.DragMove();
    }
    public static void WindowSideButtons()
    {
        static void GameLauncher(object s, RoutedEventArgs e)
        {
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            var CurrentButton = (HoyoLauncher_Controls.SideButtons.Button)s;
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

            if(Launcher != "")
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = Launcher,
                        UseShellExecute = true
                    }
                ).Dispose();
        }

        HoyoWindow.GENSHIN_IMPACT_LAUNCHER.Click += GameLauncher;
        HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER.Click += GameLauncher;
        HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER.Click += GameLauncher;
        HoyoWindow.ZZZ_LAUNCHER.Click += GameLauncher;
        HoyoWindow.TOT_LAUNCHER.Click += GameLauncher;

        HoyoWindow.GameHomePage.Click += (s,e) =>
            Process.Start(new ProcessStartInfo{ FileName = HoyoMain.CurrentGameSelected.GAME_HOMEPAGE, UseShellExecute = true }).Dispose();

        HoyoWindow.CheckInPage.Click += (s,e) =>
            Process.Start(new ProcessStartInfo{ FileName = HoyoMain.CurrentGameSelected.GAME_CHECK_IN_PAGE, UseShellExecute = true }).Dispose();
    }
    public static void WindowLaunchGame()
    {
        HoyoWindow.LaunchButton.Click += async (s,e) =>
        {
            e.Handled = true;
            var MainButton = (Button)s;

            // Window Hide
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            HoyoWindow.WindowState = WindowState.Minimized;
            HoyoWindow.ShowInTaskbar = false;
            HoyoWindow.Hide();

            // App Tray
            App.AppTray.Visible = true;
            if(HoyoMain.FirstRun)
            {
                App.AppTray.ShowBalloonTip(3);
                HoyoMain.FirstRun = false;
            }
            
            // Buttons
            MainButton.IsEnabled =
            HoyoWindow.LaunchSelection.IsEnabled = false;
            MainButton.Content = AppResources.Resources.GAME_LAUNCHED_TEXT;

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
            MainButton.Content = AppResources.Resources.GAME_DEFAULT_TEXT;
            MainButton.IsEnabled =
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
        void AutoClose(object s, RoutedEventArgs e)
        {
            var SelectedButton = (Button)s;
            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
            HoyoGames HG = null;

            switch(SelectedButton.Name)
            {
                case "GAME_SELECTION_GI" : HG = HoyoGames.GenshinImpact; break;
                case "GAME_SELECTION_HSR": HG = HoyoGames.HonkaiStarRail; break;
                case "GAME_SELECTION_HI3": HG = HoyoGames.HonkaiImpactThird; break;

                case "GAME_SELECTION_ZZZ":
                        HoyoWindow.WINDOW_BORDER.Background = new ImageBrush(new BitmapImage(new("pack://application:,,,/Resources/ZZZ.jpg", UriKind.RelativeOrAbsolute)));
                        HoyoWindow.HomeBG.Children.Remove(HoyoWindow.MainBG);
                        HoyoWindow.HomeBG.Children.Remove(HoyoWindow.HoyoTitleIMG);
                        HoyoWindow.CheckInPage.IsEnabled = true;
                        HoyoWindow.LaunchButton.IsEnabled = false;
                        HoyoWindow.LaunchButton.Content = AppResources.Resources.GAME_SOON_TEXT;
                    break;
            }

            if(HG is not null)
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