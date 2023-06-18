﻿using AppRes = AppResources.Resources;
namespace HoyoLauncher.Core;

[Events]
public sealed class EventsHandles
{
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

                        new HoyoValues()
                        {
                            Background = null,
                            RemoveMainBG = false,
                            CheckInButton = false,
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
            string Message = "";

            switch(CurrentButton.Name)
            {
                case "GENSHIN_IMPACT_LAUNCHER":
                    Launcher = Path.Combine(HoyoGames.GenshinImpact.GAME_DIRECTORY, "launcher.exe");
                    Message = "Genshin Impact Launcher is opening!";
                    break;
                
                case "HONKAI_STAR_RAIL_LAUNCHER":
                    Launcher = Path.Combine(HoyoGames.HonkaiStarRail.GAME_DIRECTORY, "launcher.exe");
                    Message = "Honkai Star Rail Launcher is opening!";
                    break;
                
                case "HONKAI_IMPACT_THIRD_LAUNCHER":
                    Launcher = Path.Combine(HoyoGames.HonkaiImpactThird.GAME_DIRECTORY, "launcher.exe");
                    Message = "Honkai Impact Launcher is opening!";
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

            if(Message is not "")
                MessageBox.Show(Message, "A Very Cool Message Box", MessageBoxButton.OK);
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

            if(Equals(HoyoWindow.LaunchButton.Content, AppRes.GAME_UPDATE_TEXT))
            {
                HoyoMain.ProcessStart(HoyoMain.ExecutableName);
                MessageBox.Show("Opening Original Launcher to Update the game!", "A Very Cool Message Box", MessageBoxButton.OK);
                return;
            }

            HoyoWindow.GameSelection.Visibility = Visibility.Hidden;

            App.AppMinimizeToTray();
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
            HoyoGames SelectedHoyoGame = null;

            switch(SelectedButton.Name)
            {
                case "GAME_SELECTION_GI" : SelectedHoyoGame = HoyoGames.GenshinImpact;        break;
                case "GAME_SELECTION_HSR": SelectedHoyoGame = HoyoGames.HonkaiStarRail;       break;
                case "GAME_SELECTION_HI3": SelectedHoyoGame = HoyoGames.HonkaiImpactThird;    break;
                case "GAME_SELECTION_ZZZ": SelectedHoyoGame = HoyoGames.ZenlessZoneZero;      break;
            }

            HoyoMain.CurrentGameSelected = SelectedHoyoGame;
            HoyoMain.GameChange(SelectedButton.Uid);
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