namespace HoyoLauncher;

public partial class App
{

    public static readonly bool IsPreview = true;

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
        if (!HoyoMain.FirstRun) return;

        AppTray.ShowBalloonTip(3);
        HoyoMain.FirstRun = false;
    }

    public static void AppMinimizeToTray()
    {
        HoyoWindow.WindowState = WindowState.Minimized;

        if (AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY is false) return;

        HoyoWindow.ShowInTaskbar = false;
        HoyoWindow.Hide();
        AppTray.Visible = true;
        AppNotification();
    }

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