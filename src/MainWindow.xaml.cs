namespace HoyoLauncherProject;

public partial class MainWindow : Window
{
    public static MainWindow Current { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        Current = this;

        HoyoLauncher.Initialize();
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
}