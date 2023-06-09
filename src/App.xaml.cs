namespace HoyoLauncher;

public partial class App : Application
{
    static Mutex _Mutex;
    //static readonly string dir = Path.Combine(Path.GetTempPath(), "HoyoverseBG");
    //public static readonly Uri TempBG = new(Path.Combine(Path.GetTempPath(), "HoyoverseBG", "bg.mp4"));
    //public static readonly MediaElement PreMediaElement = new();

    public static readonly Forms.NotifyIcon AppTray = new();

    public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

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
        AppTray.Click += AppTrayClick;

        AppTray.BalloonTipText = "HoyoLauncher will be running in the background.";
        AppTray.BalloonTipTitle = appName;
        AppTray.BalloonTipIcon = Forms.ToolTipIcon.None;

        // Due to write permissions or idfk why it wont write on the TEMP i removed it.
        //if (!Directory.Exists(dir))
        //Directory.CreateDirectory(dir);

        //if (!File.Exists(TempBG.AbsolutePath))
        //{
        //    using FileStream fs = File.Create(TempBG.AbsolutePath);

        //    foreach(byte Data in AppResources.Resources.bg.Cast<byte>())
        //    fs.WriteByte(Data);
        //}

        base.OnStartup(e);
    }

    void AppTrayClick(object s, EventArgs e)
    {
        if (MainWindow.WindowState is not WindowState.Minimized) return;

        MainWindow.Show();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.ShowInTaskbar = true;
        AppTray.Visible = false;
    }

    public static void NotifTray()
    {
        if(!HoyoMain.FirstRun) return;
        AppTray.ShowBalloonTip(3);
        HoyoMain.FirstRun = false;
    }

    public static void AppMinimizeToTray()
    {
        HoyoWindow.WindowState = WindowState.Minimized;

        if(HoyoLauncherSettings.HoyoSettings.IsMinimizeToTray is false) return;
        HoyoWindow.ShowInTaskbar = false;
        HoyoWindow.Hide();
        AppTray.Visible = true;
        NotifTray();
    }
}