namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings
{
    void ChangePageClick(object s, MouseButtonEventArgs e)
    {
        var sButton = (Border)e.Source;

        if (e.ChangedButton is not MouseButton.Left) return;

        foreach (var page in new List<Canvas>{ Locations, Others, About })
            page.Visibility = Visibility.Collapsed;

        foreach (var button in new List<Border>{ Button_Locations, Button_Others, Button_About })
        {
            ((TextBlock)button.Child).Foreground = Brushes.Black;
            button.Background = UnSelectedPageColor;
        }

        sButton.Background = SelectedPageColor;
        ((TextBlock)sButton.Child).Foreground = App.ConvertColorFromString("#997f5f");

        switch (sButton.Name)
        {
            case "Button_Locations":    Locations.Visibility = Visibility.Visible;  break;
            case "Button_Others":       Others.Visibility = Visibility.Visible;     break;
            case "Button_About":        About.Visibility = Visibility.Visible;      break;
        }
    }
}