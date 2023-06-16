namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings : Window
{

    // Hear me out, I'm kinda lazy when making the Button_Location etc.. so fuck the mess

    Brush SelectedPageColor = new BrushConverter().ConvertFromString("#f6f5f3") as Brush;
    Brush UnSelectedPageColor = new BrushConverter().ConvertFromString("#e3e1de") as Brush;

    public HoyoSettings()
    {
        InitializeComponent();

        VERSION.Text = App.Version;
        BUILDHASH.Text = $"({App.UniqueHashBUILD})";

        HoyoWindow.BLACK_THING.Opacity = 0.5;

        WindowDrag.MouseDown += (s, e) => { if (e.ChangedButton is MouseButton.Left) DragMove(); };
        ExitButton.Click   += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        Github.MouseDown += (s,e) =>
            HoyoMain.ProcessStart("https://github.com/IchimakiKasura/HoyoLauncher");

        RadioButtonTray.IsChecked = AppSettings.Settings.Default.MinimizedTray;

        RadioButtonTray_Click.MouseDown += (s, e) =>
        {
            if (e.ChangedButton is MouseButton.Left)
                AppSettings.Settings.Default.MinimizedTray = (bool)(RadioButtonTray.IsChecked = !RadioButtonTray.IsChecked);
        };

        Button_Location.MouseDown += (s, e) => ChangePage(e, location: Visibility.Visible);
        Button_Others.MouseDown += (s, e) => ChangePage(e, others: Visibility.Visible);
        Button_About.MouseDown += (s, e) => ChangePage(e, about: Visibility.Visible);
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

    void GithubHome(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura");
    void GithubLicense(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/HoyoLauncher/blob/master/LICENSE");
    void GithubProject(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/HoyoLauncher");
    void GithubPR(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/HoyoLauncher/pulls");
    void GithubIssue(object s, RoutedEventArgs e) =>
        HoyoMain.ProcessStart("https://github.com/IchimakiKasura/HoyoLauncher/issues");

    void MihoyoPage(object s, RoutedEventArgs e) =>
    HoyoMain.ProcessStart("https://www.mihoyo.com/en/");
    void HoyoversePage(object s, RoutedEventArgs e) =>
    HoyoMain.ProcessStart("https://www.hoyoverse.com/en-us/");

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

        AppSettings.Settings.Default.GENSHIN_IMPACT_DIR         = GI_DIR_TXT.Text;
        AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR       = HSR_DIR_TXT.Text;
        AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR    = HI3_DIR_TXT.Text;
        AppSettings.Settings.Default.Save();
        HoyoGames.RefreshDirectory();

        if(!ErrorOccured)
            Close();
    }

    void ChangePage(MouseButtonEventArgs e,
                    Visibility location = Visibility.Hidden,
                    Visibility others = Visibility.Hidden,
                    Visibility about = Visibility.Hidden)
    {
        if (e.ChangedButton is not MouseButton.Left) return;

        Locations.Visibility = location;
        Others.Visibility = others;
        About.Visibility = about;

        Button_Location.Background = location is Visibility.Visible ? SelectedPageColor : UnSelectedPageColor;
        Button_Others.Background = others is Visibility.Visible ? SelectedPageColor : UnSelectedPageColor;
        Button_About.Background = about is Visibility.Visible ? SelectedPageColor : UnSelectedPageColor;
    }
}