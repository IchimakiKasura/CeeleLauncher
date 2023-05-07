namespace HoyoLauncherProject;

/*
    tbh i kinda want to over engineer the coding but its just a simple launcher
    so excuse my "rushed coding" or so
*/

public partial class MainWindow : Window
{
    public static MainWindow Current { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        Current = this;

        HoyoLauncher.Initialize();

        Loaded += (s,e)=>
        new WindowTransparency(this).MakeTransparent();

        MediaElementBG.Source = new(App.TempBG);


#if !DEBUG
        AppVersion.Text = APP_VERSION;
#else
        AppVersion.Text = "DEVELOPMENT BUILD";
#endif
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = HoyoLauncher.IsGameRunning;

        if(HoyoLauncher.IsGameRunning)
            MessageBox.Show("Game is running! Cannot be closed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        base.OnClosing(e);
    }

    public void ChangeGame(string BG) =>
        MAIN_BACKGROUND.Background = new ImageBrush(new BitmapImage(new Uri(BG, UriKind.RelativeOrAbsolute)));

    private void MediaElement_OnMediaEnded(object s, RoutedEventArgs e)
    {
        MediaElement el = (MediaElement)s;

        el.Position = TimeSpan.Zero;
        el.Play();
    }
}