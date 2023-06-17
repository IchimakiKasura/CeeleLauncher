namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings : Window
{

    readonly Brush SelectedPageColor = new BrushConverter().ConvertFromString("#f6f5f3") as Brush;
    readonly Brush UnSelectedPageColor = new BrushConverter().ConvertFromString("#e3e1de") as Brush;

    public HoyoSettings()
    {
        InitializeComponent();

        VERSION.Text = App.Version;
        BUILDHASH.Text = $"({App.UniqueHashBUILD})";

        HoyoWindow.BLACK_THING.Opacity = 0.5;

        WindowDrag.MouseDown += (s, e) => { if (e.ChangedButton is MouseButton.Left) DragMove(); };
        ExitButton.Click   += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        RadioButtonTray.IsChecked = AppSettings.Settings.Default.MinimizedTray;

        RadioButtonTray_Click.MouseDown += (s, e) =>
        {
            if (e.ChangedButton is MouseButton.Left)
                AppSettings.Settings.Default.MinimizedTray = (bool)(RadioButtonTray.IsChecked = !RadioButtonTray.IsChecked);
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

    void LocationButtonClick(object s, RoutedEventArgs e)
    {
        using var Folder = new Forms.FolderBrowserDialog();
        TextBox textBox = null;

        switch(((Button)e.Source).Name)
        {
            case "GI_LOCATE":
                textBox = GI_DIR_TXT;
                break;
            case "HSR_LOCATE":
                textBox = HSR_DIR_TXT;
                break;
            case "HI3_LOCATE":
                textBox = HI3_DIR_TXT;
                break;
        }
        e.Handled = true;

        if (Folder.ShowDialog() is Forms.DialogResult.Cancel)
            return;

        textBox.Text = Folder.SelectedPath;
    }

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

    // lmao "List" was unnecessary but i like chaos
    void ChangePageClick(object s, MouseButtonEventArgs e)
    {
        var sButton = ((Border)e.Source);
        List<Canvas> pages = new() { Locations, Others, About };
        List<Border> buttons = new() { Button_Locations, Button_Others, Button_About };

        if (e.ChangedButton is not MouseButton.Left) return;

        foreach (var page in pages)
            page.Visibility = Visibility.Hidden;

        foreach (var button in buttons)
        {
            ((TextBlock)button.Child).Foreground = Brushes.Black;
            button.Background = UnSelectedPageColor;
        }

        sButton.Background = SelectedPageColor;
        ((TextBlock)sButton.Child).Foreground = (Brush)new BrushConverter().ConvertFromString("#997f5f");

        switch (sButton.Name)
        {
            case "Button_Locations":    Locations.Visibility = Visibility.Visible;  break;
            case "Button_Others":       Others.Visibility = Visibility.Visible;     break;
            case "Button_About":        About.Visibility = Visibility.Visible;      break;
        }
    }

    static void RefreshCurrentSelectedGame()
    {
        if(HoyoMain.CurrentGameSelected != HoyoGames.DEFAULT)
            HoyoMain.GameChange(--AppSettings.Settings.Default.LAST_GAME);
    }
}