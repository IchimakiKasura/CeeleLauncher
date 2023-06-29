namespace HoyoLauncher;

public partial class App
{

    public static readonly bool IsPreview = false;

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
        AppTray.ContextMenuStrip.Items.Add("Open", null, AppMenuOpen);
        AppTray.ContextMenuStrip.Items.Add("Close", null, AppMenuClose);
    }

    private void AppTrayClick(object s, Forms.MouseEventArgs e)
    {
        if (e.Button is not Forms.MouseButtons.Left) return;

        if (MainWindow.WindowState is not WindowState.Minimized) return;

        MainWindow.Show();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.ShowInTaskbar = true;
        AppTray.Visible = false;
    }

    private void AppMenuOpen(object s, EventArgs e)
    {
        MainWindow.Show();
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.ShowInTaskbar = true;
        AppTray.Visible = false;
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

        if (Config.CHECKBOX_EXIT_TRAY is false) return;

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