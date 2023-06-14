namespace HoyoLauncher.Core.Record;

public sealed record HoyoValues
{
    public ImageBrush Background { get; set; }
    public bool RemoveMainBG { get; set; }
    public bool CheckInButton { get; set; }
    public bool LaunchButton { get; set; }
    public string LaunchButtonContent { get; set; }

    public void ApplyChanges()
    {
        HoyoWindow.WINDOW_BORDER.Background = Background;
        HoyoWindow.CheckInPage.IsEnabled = CheckInButton;
        HoyoWindow.LaunchButton.IsEnabled = LaunchButton;
        HoyoWindow.LaunchButton.Content = LaunchButtonContent;

        if(RemoveMainBG)
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