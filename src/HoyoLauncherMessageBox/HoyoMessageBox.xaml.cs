namespace HoyoLauncher.HoyoLauncherMessageBox;

// Experimental

public partial class HoyoMessageBox : Window
{
    [StaticWindow]
    public static HoyoMessageBox HoyoMessageBoxStatic { get; set; }

    public HoyoMessageBox(string Caption, string Message)
    {
        InitializeComponent();

        System.Media.SystemSounds.Exclamation.Play();

        HoyoMessageBoxStatic = this;

        this.Caption.Text = Caption;
        this.Message.Text = Message;

        WindowDrag.MouseDown += App.DragMove<HoyoMessageBox>;

        OkBtn.MouseDown += (s, e) => Close();
    }

    public static void Show(string Caption, string Message, Window Owner) =>
        new HoyoMessageBox(Caption, Message){ Owner = Owner}.ShowDialog();
    
}