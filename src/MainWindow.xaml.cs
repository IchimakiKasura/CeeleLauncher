namespace HoyoLauncherProject;

public partial class MainWindow : Window
{
    public static MainWindow Current { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        Current = this;
        GameSelection.Visibility = Visibility.Hidden;

        HoyoLauncher.Initialize();
    }


    public void ChangeGame(string BG) =>
        MAIN_BACKGROUND.Background =
            new ImageBrush(new BitmapImage(new Uri(BG, UriKind.RelativeOrAbsolute)));
}