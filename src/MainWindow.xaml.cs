namespace HoyoLauncher;

public partial class MainWindow : Window
{
    public static MainWindow HoyoWindow { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        HoyoWindow = this;

        HoyoMain.Initialize();

#if !DEBUG
        AppVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#else
        AppVersion.Text = "DEVELOPMENT BUILD";
#endif
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = HoyoMain.IsGameRunning;

        if(HoyoMain.IsGameRunning)
            MessageBox.Show("Game is running! Cannot be closed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        base.OnClosing(e);
    }

    private void MediaElement_OnMediaEnded(object s, RoutedEventArgs e)
    {
        MediaElement el = (MediaElement)s;

        el.Position = TimeSpan.Zero;
        el.Play();
    }
}