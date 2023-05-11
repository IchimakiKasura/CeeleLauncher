using HoyoLauncher.Resources.Icons;

namespace HoyoLauncher;

public partial class App : Application
{
    static Mutex _Mutex;
    static readonly string dir = Path.Combine(Path.GetTempPath(), "HoyoverseBG");

    public static readonly Forms.NotifyIcon AppTray = new();
    public static readonly string TempBG = Path.Combine(Path.GetTempPath(), "HoyoverseBG", "bg.mp4");
    public static readonly MediaElement PreMediaElement = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        string appName = Assembly.GetExecutingAssembly().GetName().Name;

        _Mutex = new(true, appName, out bool createdNew);

        if (!createdNew)
            if (MessageBox.Show("Only one instance at a time!", "Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning) is MessageBoxResult.OK)
                Environment.Exit(0);

        var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
        menuDropAlignmentField.SetValue(null, !SystemParameters.MenuDropAlignment || menuDropAlignmentField is null);

        // Tray Icon
        AppTray.Icon = IconResources.Icon_16;
        AppTray.Visible = false;
        AppTray.Text = appName;
        AppTray.Click += (s, e) =>
        {
            if (MainWindow.WindowState is not WindowState.Minimized) return;

            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.ShowInTaskbar = true;
            AppTray.Visible = false;
        };

        AppTray.BalloonTipText = "HoyoLauncher will be running in the background.";
        AppTray.BalloonTipTitle = appName;
        AppTray.BalloonTipIcon = Forms.ToolTipIcon.None;

        // Extracting the BG to play
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(TempBG))
            File.WriteAllBytes(TempBG, AppResources.Resources.bg);

        base.OnStartup(e);
    }
}