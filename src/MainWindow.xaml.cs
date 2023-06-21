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
        AppVersion.Text = App.Version;
#else
        AppVersion.Text += $": test ver({App.Version})";
        Console.WriteLine("BUILD HASH: "+App.UniqueHashBUILD);
#endif
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = HoyoMain.IsGameRunning;

        if(HoyoMain.IsGameRunning)
            MessageBox.Show("Game is running! Cannot be closed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        else AppSettings.Settings.Default.Save(); 

        base.OnClosing(e);
    }

    private void MediaElement_OnMediaEnded(object s, RoutedEventArgs e)
    {
        MediaElement el = (MediaElement)s;
        
        el.Position = TimeSpan.Zero;
        el.Play();
    }
}