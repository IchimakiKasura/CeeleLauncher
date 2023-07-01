namespace HoyoLauncher;

public partial class App : Application
{
    public static MainConfig Config { get; set; } = MainConfig.Instance;

    static bool AppAlreadyOpened;
    private static readonly Mutex _Mutex = new(true, UniqueHashBUILD, out AppAlreadyOpened);
    public static bool IsFromTray { get; private set; }= false;

    protected override void OnStartup(StartupEventArgs e)
    {
        GC.KeepAlive(_Mutex);

        if (!AppAlreadyOpened)
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
}