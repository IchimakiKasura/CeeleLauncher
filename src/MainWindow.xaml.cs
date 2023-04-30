namespace HoyoLauncherProject;

public partial class MainWindow : Window
{
    public static MainWindow current;

    public MainWindow()
    {
        InitializeComponent();
        current = this;

        EventHandlers.Initialize();
        HoyoLauncher.Initialize();

        GameSelection.Visibility = Visibility.Hidden;
    }

    public void ChangeGame(string BG)
    {
        MAIN_BACKGROUND.Background =
            new ImageBrush(new BitmapImage(new Uri(BG, UriKind.RelativeOrAbsolute)));
            
    }
}