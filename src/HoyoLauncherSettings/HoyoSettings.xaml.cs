namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings : Window
{
    public static bool? IsMinimizeToTray;

    public HoyoSettings()
    {
        InitializeComponent();

        HoyoWindow.BLACK_THING.Opacity = 0.5;

        WindowDrag.MouseDown += (s, e) => { if (e.ChangedButton is MouseButton.Left) DragMove(); };
        ExitButton.Click   += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        Github.MouseDown += (s,e) =>
            Process.Start(new ProcessStartInfo{ FileName = "https://github.com/IchimakiKasura/HoyoLauncher", UseShellExecute = true }).Dispose();
            
        Github.MouseEnter += (s,e) => GithubToolTip.Visibility = Visibility.Visible;
        Github.MouseLeave += (s,e) => GithubToolTip.Visibility = Visibility.Hidden;

        RadioButtonTray.IsChecked = IsMinimizeToTray = AppSettings.Settings.Default.MinimizedTray;

        MinimizeToTray.MouseDown += (s, e) =>
        {
            if (e.ChangedButton is MouseButton.Left)
                IsMinimizeToTray = RadioButtonTray.IsChecked = !RadioButtonTray.IsChecked;
        };
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        GI_DIR_TXT.Text = AppSettings.Settings.Default.GENSHIN_IMPACT_DIR;
        HSR_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR;
        HI3_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR;
    }

    protected override void OnClosed(EventArgs e)
    {
        HoyoWindow.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }

    static void GetFolderDirectory(TextBox txt)
    {
        using var Folder = new Forms.FolderBrowserDialog();

        if(Folder.ShowDialog() == Forms.DialogResult.Cancel)
            return;

        txt.Text = Folder.SelectedPath;
    }

    void GenshinClick(object s, RoutedEventArgs e) =>
        GetFolderDirectory(GI_DIR_TXT);
    void HSRClick(object s, RoutedEventArgs e) =>
        GetFolderDirectory(HSR_DIR_TXT);
    void HI3Click(object s, RoutedEventArgs e) =>
        GetFolderDirectory(HI3_DIR_TXT);

    void ConfirmClick(object s, RoutedEventArgs e)
    {
        bool ErrorOccured = false;

        List<(string config, HoyoGames AbsoluteName, HoyoButton Launcher)> GameConfigs = new()
        {
            (GI_DIR_TXT.Text, HoyoGames.GenshinImpact, HoyoWindow.GENSHIN_IMPACT_LAUNCHER),
            (HSR_DIR_TXT.Text, HoyoGames.HonkaiStarRail, HoyoWindow.HONKAI_STAR_RAIL_LAUNCHER),
            (HI3_DIR_TXT.Text, HoyoGames.HonkaiImpactThird, HoyoWindow.HONKAI_IMPACT_THIRD_LAUNCHER)
        };

        foreach(var (config, name, Launcher) in CollectionsMarshal.AsSpan(GameConfigs))
        {
            HoyoMain.ValidateSettings(config, name, Launcher, out ErrorOccured);
            if(ErrorOccured) break;
        }

        AppSettings.Settings.Default.GENSHIN_IMPACT_DIR = GI_DIR_TXT.Text;
        AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR = HSR_DIR_TXT.Text;
        AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR = HI3_DIR_TXT.Text;
        AppSettings.Settings.Default.MinimizedTray = (bool)IsMinimizeToTray;
        AppSettings.Settings.Default.Save();
        HoyoGames.RefreshDirectory();

        if(!ErrorOccured)
            Close();
    }
}