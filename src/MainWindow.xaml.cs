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
        AppVersion.Text = App.Version;
#else
        AppVersion.Text = $"DEVELOPMENT BUILD: ver({App.Version})";
#endif

        Loaded += delegate
        {
            if(!AppSettings.Settings.Default.FIRSTRUN)
                new FirstRunWindow { Owner = HoyoWindow }.ShowDialog();
        };
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = HoyoMain.IsGameRunning;

        if(HoyoMain.IsGameRunning)
            MessageBox.Show("Game is running! Cannot be closed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        base.OnClosing(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        AppSettings.Settings.Default.Save();
        base.OnClosed(e);
    }

    private void MediaElement_OnMediaEnded(object s, RoutedEventArgs e)
    {
        MediaElement el = (MediaElement)s;
        
        el.Position = TimeSpan.Zero;
        el.Play();
    }
}