namespace HoyoLauncher.HoyoLauncherMessageBox;

// Experimental
public partial class HoyoMessageBox : Window
{
    [StaticWindow]
    public static HoyoMessageBox HoyoMessageBoxStatic { get; set; }

    Storyboard storyboard;
    DoubleAnimation opac;

    public HoyoMessageBox(string Caption, string Message)
    {
        InitializeComponent();

        PlayOpenAnimation();

        System.Media.SystemSounds.Exclamation.Play();

        HoyoMessageBoxStatic = this;

        this.Caption.Text = Caption;
        this.Message.Text = Message;

        WindowDrag.MouseDown += App.DragMove<HoyoMessageBox>;

        OkBtn.MouseDown += OK_BUTTON;
    }

    public void OK_BUTTON(object sender, MouseButtonEventArgs events)
    {
        if (events.ChangedButton is MouseButton.Right) return;

        opac = new(1, 0, TimeSpan.FromMilliseconds(100));

        Play(Close);
    }

    public void PlayOpenAnimation()
    {
        opac = new(0, 1, TimeSpan.FromMilliseconds(50));
        Play();
    }

    public void Play([Optional]Action method)
    {
        Storyboard.SetTargetProperty(opac, new("Opacity"));
        Storyboard.SetTarget(opac, this);
        storyboard = new() { Children = new() { opac } };

        if(method is not null)
            storyboard.Completed += (s, e) => method();

        storyboard.Begin();
    }

    public static void Show(string Caption, string Message, Window Owner) =>
        new HoyoMessageBox(Caption, Message){ Owner = Owner}.ShowDialog();
}