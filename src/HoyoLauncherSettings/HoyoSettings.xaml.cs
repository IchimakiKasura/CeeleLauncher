namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings : Window
{

    // Hear me out, I'm kinda lazy when making the Button_Location etc.. so fuck the mess
    readonly Brush SelectedPageColor = new BrushConverter().ConvertFromString("#f6f5f3") as Brush;
    readonly Brush UnSelectedPageColor = new BrushConverter().ConvertFromString("#e3e1de") as Brush;

    public HoyoSettings()
    {
        InitializeComponent();

        VERSION.Text = App.Version;
        BUILDHASH.Text = $"({App.UniqueHashBUILD})";

        HoyoWindow.BLACK_THING.Opacity = 0.5;

        WindowDrag.MouseDown += (s, e) => {  };
        ExitButton.Click   += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        RadioButtonTray.IsChecked = AppSettings.Settings.Default.MinimizedTray;

        RadioButtonTray_Click.MouseDown += (s, e) =>
        {
            if (e.ChangedButton is MouseButton.Left)
                AppSettings.Settings.Default.MinimizedTray = (bool)(RadioButtonTray.IsChecked = !RadioButtonTray.IsChecked);
        };

        Button_Location.MouseDown += (s, e) =>  ChangePage(e, location: 0);
        Button_Others.MouseDown += (s, e) =>    ChangePage(e, others: 0);
        Button_About.MouseDown += (s, e) =>     ChangePage(e, about: 0);
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

        if(Folder.ShowDialog() is Forms.DialogResult.Cancel)
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
            HoyoMain.ValidateSettings(config, name, Launcher, out bool IsInvalidGame);

            if(IsInvalidGame)
            {
                MessageBox.Show($"ERROR:\n\nThe \"{name.GAME_NAME}\" config cannot be found!\n or its an incorrect game.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorOccured = true;
            }
        }

        AppSettings.Settings.Default.GENSHIN_IMPACT_DIR         = GI_DIR_TXT.Text;
        AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR       = HSR_DIR_TXT.Text;
        AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR    = HI3_DIR_TXT.Text;
        AppSettings.Settings.Default.Save();
        HoyoGames.RefreshDirectory();

        RefreshCurrentSelectedGame();
        
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

    static void RefreshCurrentSelectedGame()
    {
        if(HoyoMain.CurrentGameSelected != HoyoGames.DEFAULT)
            HoyoMain.GameChange(--AppSettings.Settings.Default.LAST_GAME);
    }
}