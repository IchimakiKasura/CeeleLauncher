namespace HoyoLauncher.Core;

public sealed class HoyoChange
{
    public static void SetValues(HoyoValues values)
    {
        HoyoWindow.WINDOW_BORDER.Background = values.Background;
        HoyoWindow.CheckInPage.IsEnabled = values.CheckInButton;
        HoyoWindow.LaunchButton.IsEnabled = values.LaunchButton;
        HoyoWindow.LaunchButton.Content = values.LaunchButtonContent;

        if(values.RemoveMainBG)
        {
            HoyoWindow.HomeBG.Children.Remove(HoyoWindow.MainBG);
            HoyoWindow.HomeBG.Children.Remove(HoyoWindow.HoyoTitleIMG);
        }
        else
        {
            HoyoWindow.HomeBG.Children.Add(HoyoWindow.MainBG);
            HoyoWindow.HomeBG.Children.Add(HoyoWindow.HoyoTitleIMG);
        }
    }
}