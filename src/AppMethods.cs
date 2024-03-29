namespace HoyoLauncher;

public partial class App
{

#if IsPreviewBuild
    public static readonly bool IsPreview = true;
#else
    public static readonly bool IsPreview = false;
#endif

    static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;
    public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    public readonly static string UniqueHashBUILD = HoyoMain.GenerateMD5HASH();
    public static Brush ConvertColorFromString(string color) => new BrushConverter().ConvertFromString(color) as Brush;
    public static readonly Forms.NotifyIcon AppTray = new();

    private void AppInitializeTray()
    {
        AppTray.Icon = IconResources.Icon_16;
        AppTray.Visible = false;
        AppTray.Text = AppName;
        AppTray.MouseDown += AppTrayClick;

        AppTray.BalloonTipText = "Ceele will be running in the background.";
        AppTray.BalloonTipTitle = AppName;
        AppTray.BalloonTipIcon = Forms.ToolTipIcon.None;

        AppTray.ContextMenuStrip = new();
        AppTray.ContextMenuStrip.Show();
        AppTray.ContextMenuStrip.Items.Add(new Forms.ToolStripLabel($"{AppName} ({Version})") { ForeColor = Draw.Color.Gray } );
        AppTray.ContextMenuStrip.Items.Add("-");
        AppTray.ContextMenuStrip.Items.Add("Open mainwindow", null, AppMenuOpen);
        AppTray.ContextMenuStrip.Items.Add("Exit", null, AppMenuClose);
    }

    private void AppTrayClick(object s, Forms.MouseEventArgs e)
    {
        if (e.Button is not Forms.MouseButtons.Left || MainWindow.WindowState is not WindowState.Minimized)
            return;

        MainWindow.Show();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.ShowInTaskbar = true;
        AppTray.Visible = false;
        MainWindow.Activate();
    }

    private void AppMenuOpen(object s, EventArgs e)
    {
        MainWindow.Show();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.ShowInTaskbar = true;
        AppTray.Visible = false;
        MainWindow.Activate();
    }

    private void AppMenuClose(object s, EventArgs e)
    {
        IsFromTray = true;
        HoyoWindow.Close();
    }

    private static void AppNotification()
    {
        if (!HoyoMain.FirstRun) return;

        AppTray.ShowBalloonTip(3);
        HoyoMain.FirstRun = false;
    }

    public static void AppMinimizeToTray()
    {
        HoyoWindow.WindowState = WindowState.Minimized;
        HoyoWindow.ShowInTaskbar = false;
        HoyoWindow.Hide();

        AppTray.Visible = true;
        AppNotification();
    }

    // Best I can do.
    // A dynamic DragMove for every window
    // (that has attributes placed on static instance of the Window ☠️)
    public static void DragMove<T>(object s, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is not MouseButton.Left) return;

        Type type = Assembly.GetExecutingAssembly().GetType(typeof(T).ToString());
        
        if (type.IsSubclassOf(typeof(Window)))
            foreach (PropertyInfo propertyInfo in type.GetProperties())
                if (propertyInfo.GetCustomAttributes(typeof(StaticWindowAttribute), true).Length is 1)
                {
                    var propertValue = propertyInfo.GetValue(null); 
                    propertValue.GetType().GetMethod("DragMove").Invoke(propertValue, null);
                }
    }
}