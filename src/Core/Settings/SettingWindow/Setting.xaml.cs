namespace HoyoLauncherProject.Core.SettingWindow;

public partial class Setting : Window
{
    public Setting()
    {
        InitializeComponent();

        MainWindow.Current.BLACK_THING.Opacity = 0.5;

        ExitButton.Click += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();
        ConfirmButton.Click += (s, e) => Settings.HoyoLauncher.Default.Save();
    }

    protected override void OnClosed(EventArgs e)
    {
        MainWindow.Current.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }
}
