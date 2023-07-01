namespace HoyoLauncher.HoyoLauncherMessageBox;

public partial class HoyoMessageBox : Window
{
    [StaticWindow]
    public static HoyoMessageBox HoyoMessageBoxStatic { get; set; }

    Storyboard storyboard;
    DoubleAnimation opacity,scaleX,scaleY;
    readonly SineEase Ease = new() { EasingMode = EasingMode.EaseInOut };

    public HoyoMessageBox(string Caption, string Message)
    {
        InitializeComponent();
        PlayOpenAnimation();

        Title = Caption;
        this.Caption.Text = Caption;
        this.Message.Text = Message;

        WindowStartupLocation = Owner is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
        ShowInTaskbar = !HoyoWindow.ShowInTaskbar;

        HoyoMessageBoxStatic = this;

        Loaded += (s, e) =>
        {
            UpdateLayout();

            if (Message_Border.ActualHeight is 70) return;

            var NewHeight = Message_Border.ActualHeight - 70;
            Height += NewHeight;
            Canvas.SetTop(OkBtn, Canvas.GetTop(OkBtn) + NewHeight);
        };

        WindowDrag.MouseDown += App.DragMove<HoyoMessageBox>;

        OkBtn.MouseDown += OK_BUTTON;

    }

    public void OK_BUTTON(object sender, MouseButtonEventArgs events)
    {
        if (events.ChangedButton is MouseButton.Right) return;

        scaleY = new(1, 0.8, TimeSpan.FromMilliseconds(100));
        scaleX = new(1, 0.8, TimeSpan.FromMilliseconds(100));
        opacity = new(1, 0, TimeSpan.FromMilliseconds(150));
        Play(Close);
    }

    public void PlayOpenAnimation()
    {
        scaleY = new(0.8, 1, TimeSpan.FromMilliseconds(100));
        scaleX = new(0.8, 1, TimeSpan.FromMilliseconds(100));
        opacity = new(0, 1, TimeSpan.FromMilliseconds(150));
        Play();
        System.Media.SystemSounds.Exclamation.Play();
    }

    public void Play([Optional]Action method)
    {
        scaleX.EasingFunction = Ease;
        scaleY.EasingFunction = Ease;
        opacity.EasingFunction = Ease;

        Storyboard.SetTargetProperty(scaleX, new("RenderTransform.(ScaleTransform.ScaleX)"));
        Storyboard.SetTargetProperty(scaleY, new("RenderTransform.(ScaleTransform.ScaleY)"));

        Storyboard.SetTargetProperty(opacity, new("Opacity"));

        Storyboard.SetTarget(opacity, this);

        Storyboard.SetTarget(scaleX, MainWindow);
        Storyboard.SetTarget(scaleY, MainWindow);
        storyboard = new() { Children = new() { opacity, scaleX, scaleY } };

        if (method is not null)
            storyboard.Completed += (s, e) => method();

        storyboard.Begin();
    }

    public static void Show(string Caption, string Message, [Optional]Window Owner) =>
        new HoyoMessageBox(Caption, Message){ Owner = Owner }.ShowDialog();
}