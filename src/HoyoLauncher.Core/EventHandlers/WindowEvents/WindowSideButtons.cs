namespace HoyoLauncher.Core.EventHandlers.WindowEvents;

[Events]
public sealed class WindowSideButtons
{
    public static void Method() =>
        HoyoWindow.SideButton_Click.AddHandler(
            System.Windows.Controls.Primitives.ButtonBase.ClickEvent,
            new RoutedEventHandler(EventClick)
        );

    private static void EventClick(object s, RoutedEventArgs e)
    {
        HoyoWindow.GameSelection.Visibility = Visibility.Hidden;
        var CurrentButton = (HoyoButton)e.Source;

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

}