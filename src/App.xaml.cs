using System.Windows.Interop;

namespace HoyoLauncher;

public partial class App : Application
{
    public static MainConfig Config { get; set; } = MainConfig.Instance;

    static bool AppAlreadyOpened;
    private static Mutex _Mutex = null;
    public static bool IsFromTray { get; private set; } = false;

    protected override void OnStartup(StartupEventArgs e)
    {
        _Mutex = new(true, UniqueHashBUILD, out AppAlreadyOpened);

        GC.KeepAlive(_Mutex);

        // FIXME: Help, Is anyone can open the existing window?
        // I did it using hWnd and only opens if the window is minimized and taskbar visible
        // doesnt work if its on System Tray. 
        if (!AppAlreadyOpened)
            if (MessageBox.Show("Only one instance at a time!",
                                "Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning) is MessageBoxResult.OK)
                Environment.Exit(0);

        AppInitializeTray();

        var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
        menuDropAlignmentField.SetValue(null, !SystemParameters.MenuDropAlignment || menuDropAlignmentField is null);

        base.OnStartup(e);
    }
}