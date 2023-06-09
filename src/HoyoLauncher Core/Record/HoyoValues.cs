namespace HoyoLauncher.Core.Record;

public sealed record HoyoValues
{
    public ImageBrush Background { get; set; }
    public bool RemoveMainBG { get; set; }
    public bool CheckInButton { get; set; }
    public bool LaunchButton { get; set; }
    public string LaunchButtonContent { get; set; }

    public HoyoValues(
        ImageBrush _Background,
        bool _RemoveMainBG,
        bool _CheckInButton,
        bool _LaunchButton,
        string _LaunchButtonContent
    )
    {
        Background = _Background;
        RemoveMainBG = _RemoveMainBG;
        CheckInButton = _CheckInButton;
        LaunchButton = _LaunchButton;
        LaunchButtonContent = _LaunchButtonContent;
    }
}