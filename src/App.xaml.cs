namespace HoyoLauncher;

public partial class App : Application
{
    static bool createdNew;

    static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;
    public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    public readonly static string UniqueHashBUILD = HoyoMain.GenerateMD5HASH();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
    private static readonly Mutex _Mutex = new(true, AppName+Version, out createdNew);

    public static readonly Forms.NotifyIcon AppTray = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!createdNew)
            if (MessageBox.Show("Only one instance at a time!",
                                "Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning) is MessageBoxResult.OK)
                Environment.Exit(0);

        var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
        menuDropAlignmentField.SetValue(null, !SystemParameters.MenuDropAlignment || menuDropAlignmentField is null);

        AppInitializeTray();

        base.OnStartup(e);
    }

    private void AppInitializeTray()
    {
        AppTray.Icon = IconResources.Icon_16;
        AppTray.Visible = false;
        AppTray.Text = AppName;
        AppTray.Click += AppTrayClick;

        AppTray.BalloonTipText = "HoyoLauncher will be running in the background.";
        AppTray.BalloonTipTitle = AppName;
        AppTray.BalloonTipIcon = Forms.ToolTipIcon.None;
    }

    private void AppTrayClick(object s, EventArgs e)
    {
        if (MainWindow.WindowState is not WindowState.Minimized) return;

        MainWindow.Show();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.ShowInTaskbar = true;
        AppTray.Visible = false;
    }

    private static void AppNotification()
    {
        if(!HoyoMain.FirstRun) return;

        AppTray.ShowBalloonTip(3);
        HoyoMain.FirstRun = false;
    }

    public static void AppMinimizeToTray()
    {
        HoyoWindow.WindowState = WindowState.Minimized;

        if(AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY is false) return;

        HoyoWindow.ShowInTaskbar = false;
        HoyoWindow.Hide();
        AppTray.Visible = true;
        AppNotification();
    }
}