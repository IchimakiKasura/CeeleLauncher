namespace HoyoLauncherProject.Core.Settings.SettingWindow;

/// <summary>
/// Interaction logic for Settings.xaml
/// </summary>
public partial class Settings : Window
{
    public Settings()
    {
        InitializeComponent();

        MainWindow.Current.BLACK_THING.Opacity = 0.5;

        ExitButton.Click += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();
    }

    protected override void OnClosed(EventArgs e)
    {
        MainWindow.Current.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }
}
