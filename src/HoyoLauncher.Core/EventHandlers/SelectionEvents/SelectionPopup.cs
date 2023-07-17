namespace HoyoLauncher.Core.EventHandlers.SelectionEvents;

[Events]
public sealed class SelectionPopup
{
    static readonly SineEase EaseMode = new() { EasingMode = EasingMode.EaseInOut };
    static readonly DoubleAnimation HeightAnimation = new()
    {
        EasingFunction = EaseMode,
        Duration = TimeSpan.FromMilliseconds(250)
    };

    // Animation might be buggy when clicking it too fast.
    public static void Method() =>
        HoyoWindow.LaunchSelection.Click += EventClick;

    private static void EventClick(object s, RoutedEventArgs e)
    {
        Action AnimationMethod =
            HoyoWindow.GameSelection.Visibility is not Visibility.Visible ? OpenAnimation : CloseAnimation;

        AnimationMethod();
    }
    
    private static void ChangeVisibility() =>
        HoyoWindow.GameSelection.Visibility =
            HoyoWindow.GameSelection.IsVisible ? Visibility.Collapsed : Visibility.Visible;

    private static void OpenAnimation()
    {
        HeightAnimation.From = 0;
        HeightAnimation.To = 218;

        Begin(MethodPostStart: ChangeVisibility);
    }

    private static void CloseAnimation()
    {
        HeightAnimation.From = 218;
        HeightAnimation.To = 0;

        Begin(ChangeVisibility);
    }

    private static void Begin([Optional]Action MethodComplete, [Optional]Action MethodPostStart)
    {
        Storyboard.SetTarget(HeightAnimation, HoyoWindow.GameSelection);
        Storyboard.SetTargetProperty(HeightAnimation, new("Height"));

        Storyboard storyboard = new()
        {
            Children = new()
            {
                HeightAnimation
            }
        };

        if(MethodComplete is not null)
            storyboard.Completed += (s,e) => MethodComplete();

        if(MethodPostStart is not null)
            MethodPostStart();

        storyboard.Begin();
    }
}