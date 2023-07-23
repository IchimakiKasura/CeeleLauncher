namespace HoyoLauncher.HoyoLauncherMessageBox;

public partial class HoyoMessageBox : Window
{
    [StaticWindow]
    public static HoyoMessageBox HoyoMessageBoxStatic { get; set; }

    Storyboard storyboard;
    DoubleAnimation opacity, scaleX, scaleY;
    readonly SineEase Ease = new() { EasingMode = EasingMode.EaseInOut };

    readonly TimeSpan animationTime = TimeSpan.FromMilliseconds(100),
                      animationTime50 = TimeSpan.FromMilliseconds(150);

    public HoyoMessageBox(string Caption, string Message, Window Owner)
    {
        InitializeComponent();
        HoyoMessageBoxStatic = this;

        Title = Caption;
        this.Caption.Text = Caption;
        this.Message.Text = Message;
        this.Owner        = Owner;

        WindowStartupLocation = Owner is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
        ShowInTaskbar = Owner is null;

        PlayOpenAnimation();
        
        Loaded += delegate
        {
            UpdateLayout();

            if (Message_Border.ActualHeight is 70) return;

            var NewHeight = Message_Border.ActualHeight - 70;
            Height += NewHeight;
            Canvas.SetTop(OkBtn, Canvas.GetTop(OkBtn) + NewHeight);

            Top -= NewHeight / 2;
        };

        WindowDrag.MouseDown += App.DragMove<HoyoMessageBox>;

        OkBtn.MouseDown += OK_BUTTON;

        KeyDown += (s, e) =>
        {
            if (e.Key is Key.Enter)
                OK_BUTTON(this, null);
        };
    }

    public void OK_BUTTON(object sender, MouseButtonEventArgs events)
    {
        if (events?.ChangedButton is MouseButton.Right) return;

        scaleY = new(1, 0.8, animationTime);
        scaleX = new(1, 0.8, animationTime);
        opacity = new(1, 0, animationTime50);
        Play(CloseResult);
    }

    public void PlayOpenAnimation()
    {
        scaleY = new(0.8, 1, animationTime);
        scaleX = new(0.8, 1, animationTime);
        opacity = new(0, 1, animationTime50);
        Play();
        System.Media.SystemSounds.Exclamation.Play();
    }

    public void Play([Optional] Action method)
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

    void CloseResult() =>
        DialogResult = true;
    
    public static bool? Show(string Caption, string Message, [Optional] Window Owner) =>
        new HoyoMessageBox(Caption, Message, Owner ?? null).ShowDialog();
}