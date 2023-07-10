namespace HoyoLauncher.Core.Record;

public sealed record HoyoValues
{
    public ImageBrush Background { get; set; }
    public bool RemoveMainBG { get; set; }
    public bool LaunchButton { get; set; }
    public string LaunchButtonContent { get; set; }
    public Visibility VersionBubble { get; set; }
    public Visibility PreInstall { get; set; }

    public HoyoValues(HoyoValues hoyo)
    {
        Background = hoyo.Background;
        RemoveMainBG = hoyo.RemoveMainBG;
        LaunchButton = hoyo.LaunchButton;
        LaunchButtonContent = hoyo.LaunchButtonContent;
        VersionBubble = hoyo.VersionBubble;
        PreInstall = hoyo.PreInstall;
    }

    public void ApplyChanges()
    {
        HoyoWindow.MainBG.Background = Background;
        HoyoWindow.LaunchButton.IsEnabled = LaunchButton;
        HoyoWindow.LaunchButton.Content = LaunchButtonContent;
        HoyoWindow.VERSION_BUBBLE.Visibility = VersionBubble;
        HoyoWindow.PreDownload.Visibility = PreInstall;

        if(RemoveMainBG && HoyoWindow.HomeBG.Children.Contains(HoyoWindow.HoyoTitleIMG))
        {
            HoyoWindow.MainBG.Child = null;
            HoyoWindow.HomeBG.Children.Remove(HoyoWindow.HoyoTitleIMG);
        }
        
        if (!RemoveMainBG && !HoyoWindow.HomeBG.Children.Contains(HoyoWindow.HoyoTitleIMG))
        {
            HoyoWindow.MainBG.Child = HoyoWindow.MainBGVideo;
            HoyoWindow.HomeBG.Children.Add(HoyoWindow.HoyoTitleIMG);
        }
    }
}