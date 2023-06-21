using HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;

namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings : Window
{
    [StaticWindow]
    public static HoyoSettings _hoyosettings { get; set; }

    readonly Brush SelectedPageColor = App.ConvertColorFromString("#f6f5f3");
    readonly Brush UnSelectedPageColor = App.ConvertColorFromString("#e3e1de");

    public HoyoSettings()
    {
        InitializeComponent();
        _hoyosettings = this;

        VERSION.Text = App.Version;
        BUILDHASH.Text = $"({App.UniqueHashBUILD})";
        Tooltip_Text.Text = "";

        HoyoWindow.BLACK_THING.Visibility = Visibility.Visible;

        WindowDrag.MouseDown += App.DragMove<HoyoSettings>;

        ExitButton.Click   += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        HoyoTooltips.SetToolTips();
        HoyoRadioButtons.SetRadioButtons();

        foreach (var btn in new List<Border>{ Button_Locations, Button_Others, Button_About })
        {
            btn.MouseEnter += (s,e) => ((TextBlock)btn.Child).Foreground = App.ConvertColorFromString("#f4cb99");
            btn.MouseLeave += (s,e) =>
            {
                if(btn.Background.ToString() == "#FFF6F5F3")
                    ((TextBlock)btn.Child).Foreground = App.ConvertColorFromString("#997f5f");
                else ((TextBlock)btn.Child).Foreground = Brushes.Black;
            };
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        GI_DIR_TXT.Text = AppSettings.Settings.Default.GENSHIN_IMPACT_DIR;
        HSR_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR;
        HI3_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR;
    }

    protected override void OnClosed(EventArgs e)
    {
        HoyoWindow.BLACK_THING.Visibility = Visibility.Collapsed;
        base.OnClosed(e);
    }

    void LocationButtonClick(object s, RoutedEventArgs e)
    {
        using var Folder = new Forms.FolderBrowserDialog();
        TextBox textBox = null;

        switch(((Button)e.Source).Name)
        {
            case "GI_LOCATE": textBox = GI_DIR_TXT; break;
            case "HSR_LOCATE": textBox = HSR_DIR_TXT; break;
            case "HI3_LOCATE": textBox = HI3_DIR_TXT; break;
        }
        e.Handled = true;

        if (Folder.ShowDialog() is Forms.DialogResult.Cancel)
            return;

        textBox.Text = Folder.SelectedPath;
    }

    void ConfirmClick(object s, RoutedEventArgs e)
    {
        bool ErrorOccured = false;

        List<(string config, HoyoGames AbsoluteName)> GameConfigs = new()
        {
            (GI_DIR_TXT.Text, HoyoGames.GenshinImpact),
            (HSR_DIR_TXT.Text, HoyoGames.HonkaiStarRail),
            (HI3_DIR_TXT.Text, HoyoGames.HonkaiImpactThird)
        };

        foreach(var (config, name) in CollectionsMarshal.AsSpan(GameConfigs))
        {
            HoyoMain.ValidateSettings(config, name, out bool IsInvalidGame);

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
        var sButton = (Border)e.Source;

        if (e.ChangedButton is not MouseButton.Left) return;

        foreach (var page in new List<Canvas>{ Locations, Others, About })
            page.Visibility = Visibility.Collapsed;

        foreach (var button in new List<Border>{ Button_Locations, Button_Others, Button_About })
        {
            ((TextBlock)button.Child).Foreground = Brushes.Black;
            button.Background = UnSelectedPageColor;
        }

        sButton.Background = SelectedPageColor;
        ((TextBlock)sButton.Child).Foreground = App.ConvertColorFromString("#997f5f");

        switch (sButton.Name)
        {
            case "Button_Locations":    Locations.Visibility = Visibility.Visible;  break;
            case "Button_Others":       Others.Visibility = Visibility.Visible;     break;
            case "Button_About":        About.Visibility = Visibility.Visible;      break;
        }
    }

    void ResetLocationButtonClick(object s, RoutedEventArgs e)
    {
        GI_DIR_TXT.Text = "";
        HSR_DIR_TXT.Text = "";
        HI3_DIR_TXT.Text = "";

        MessageBox.Show("Locations are now cleared!", HoyoWindow.Title);
    }

    void ResetSettingsButtonClick(object s, RoutedEventArgs e)
    {
        AppSettings.Settings.Default.Reset();

        GI_DIR_TXT.Text = AppSettings.Settings.Default.GENSHIN_IMPACT_DIR;
        HSR_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR;
        HI3_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR;

        RadioButtonTray.IsChecked = AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY;
        RadioButtonBackground.IsChecked = AppSettings.Settings.Default.CHECKBOX_BACKGROUND;
        RadioButtonSelectiveStartup.IsChecked = AppSettings.Settings.Default.CHECKBOX_LASTGAME;
        RadioButtonDisableTitle.IsChecked = AppSettings.Settings.Default.CHECKBOX_TITLE;

        MessageBox.Show("Settings has been Reset!", HoyoWindow.Title);
    }

    static void RefreshCurrentSelectedGame()
    {
        if(HoyoMain.CurrentGameSelected != HoyoGames.DEFAULT)
            GameChange.SetGame(--AppSettings.Settings.Default.LAST_GAME);
    }
}