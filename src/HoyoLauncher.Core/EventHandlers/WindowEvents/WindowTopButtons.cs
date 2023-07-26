namespace HoyoLauncher.Core.EventHandlers.WindowEvents;

[Events]
public sealed class WindowTopButtons
{
    public static void Method()
    {
        List<Button> HoyoWindowButtons = new()
        {
            HoyoWindow.ExitButton,
            HoyoWindow.MinButton,
            HoyoWindow.SettingsButton,
            HoyoWindow.HomeButton,
            HoyoWindow.RefreshButton
        };

        foreach(Button button in CollectionsMarshal.AsSpan(HoyoWindowButtons))
            button.Click += EventClick;

        HoyoWindow.TopBorder.MouseDown += App.DragMove<MainWindow>;
    }

    private static void EventClick(object sender, RoutedEventArgs events)
    {
        HoyoWindow.GameSelection.Visibility = Visibility.Collapsed;
        var CurrentButton = sender as Button;

        switch(CurrentButton.Name)
        {
            case "ExitButton":      HoyoWindow.Close();                             break;
            case "MinButton":       HoyoWindow.WindowState = WindowState.Minimized; break;
            case "SettingsButton":  ShowSettings(HoyoWindow);                       break;
            case "HomeButton":      HomeButton();                                   break;
            case "RefreshButton":   RefreshButton();                                break;
        }
    }

    private static void HomeButton()
    {
        new HoyoValues()
        {
            Background = DefaultBG.DEFAULT,
            RemoveMainBG = false,
            LaunchButton = false,
            LaunchButtonContent = "Welcome",
            VersionBubble = Visibility.Collapsed,
            PreInstall = Visibility.Collapsed,
        }
        .ApplyChanges();

        HoyoMain.CurrentGameSelected = HoyoGames.DEFAULT;
        HoyoMain.RefreshSideButtons();
        App.Config.LAST_GAME = 0;
    }

    private static async void RefreshButton()
    {
        if (HoyoMain.CurrentGameSelected == HoyoGames.DEFAULT) return;

        Debug.Write($"Removing Cache");
        HoyoMain.CurrentGameSelected.API_CACHE = null;
        HoyoMain.CurrentGameSelected.AlreadyFetch = false;
        GameChange.SetGame(--App.Config.LAST_GAME);
        Debug.WriteLine($".....DONE");

        Debug.Write($"Refreshing App Config");
        App.Config = await MainConfig.ReadConfig();
        Debug.WriteLine($".....DONE");
    }
}