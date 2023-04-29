namespace HoyoLauncherProject;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();

        HoyoLauncher.IsGenshin = true;

        EventHandlers.Initialize(this);
        HoyoLauncher.Initialize(this);
    }

    public void ChangeBG(string BG)
    {
        MAIN_BACKGROUND.Background =
            new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetFiles(BG + "\\bg")[0], UriKind.RelativeOrAbsolute))
            };
    }
}