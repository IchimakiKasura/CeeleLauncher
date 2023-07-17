namespace HoyoLauncher.Core.EventHandlers.SelectionEvents;

[Events]
public sealed class Selection__
{
    public static void Method() =>
        HoyoWindow.GameSelection_Click.AddHandler(
            System.Windows.Controls.Primitives.ButtonBase.ClickEvent,
            new RoutedEventHandler(EventClick)
        );

    private static void EventClick(object s, RoutedEventArgs e)
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
}