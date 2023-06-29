namespace HoyoLauncher;

public partial class MainWindow : Window
{
    [StaticWindow]
    public static MainWindow HoyoWindow { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        HoyoWindow = this;

        HoyoMain.Initialize();

#if !DEBUG
        AppVersion.Text = App.IsPreview ? App.Version+" PREVIEW BUILD | NOT OFFICIAL VERSION" : App.Version;
#else
        AppVersion.Text += $": test ver({App.Version})";
        Console.WriteLine("BUILD HASH: "+App.UniqueHashBUILD);
#endif
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = HoyoMain.IsGameRunning;

        if(HoyoMain.IsGameRunning)
            HoyoMessageBox.Show("Warning", "Game is running! Cannot be closed.", HoyoWindow);
        else if (App.Config.CHECKBOX_EXIT_TRAY && !App.IsFromTray)
        {
            App.AppMinimizeToTray();
            e.Cancel = true;
        }
        else App.Config.SaveConfig();

        base.OnClosing(e);
    }

    private void MediaElement_OnMediaEnded(object s, RoutedEventArgs e)
    {
        MediaElement el = (MediaElement)s;
        
        el.Position = TimeSpan.Zero;
        el.Play();
    }
}