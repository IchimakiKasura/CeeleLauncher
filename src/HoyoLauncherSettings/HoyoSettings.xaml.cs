global using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;
using HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;

namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings : Window
{
    [StaticWindow]
    public static HoyoSettings HoyoSettingStatic { get; set; }

    readonly Brush SelectedPageColor = App.ConvertColorFromString("#f6f5f3");
    readonly Brush UnSelectedPageColor = App.ConvertColorFromString("#e3e1de");

    // Indexer for RadioButtons, because I cant use REF on a fucken iteration
    public readonly BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Instance;
    public RadioButton this[string name]
    {
        get => (RadioButton)HoyoSettingStatic.GetType().GetField(name, _flags).GetValue(this);
        set => HoyoSettingStatic.GetType().GetField(name, _flags).SetValue(this, value);
    }

    public HoyoSettings()
    {
        InitializeComponent();
        HoyoSettingStatic = this;

        VERSION.Text = App.Version;

        BUILDHASH.Text = 
            App.IsPreview ? $"({App.UniqueHashBUILD}) PREVIEW BUILD | NOT OFFICIAL VERSION" : $"({App.UniqueHashBUILD})";
        
        Tooltip_Text.Text = "";

        HoyoWindow.BLACK_THING.Visibility = Visibility.Visible;

        WindowDrag.MouseDown += App.DragMove<HoyoSettings>;

        ExitButton.Click   += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        HoyoTooltips.SetToolTips();
        HoyoRadioButtons.SetRadioButtons();
        HoyoSettingsButtons.SetButtons();
        _ = new Funi.Fun(this);

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
        Settings_ScrollBar.Height = 270;

        GI_DIR_TXT.Text = App.Config.GI_DIR;
        HSR_DIR_TXT.Text = App.Config.HSR_DIR;
        HI3_DIR_TXT.Text = App.Config.HI3_DIR;
        
        RadioButtonBackground.IsChecked = App.Config.CHECKBOX_BACKGROUND;
        RadioButtonSelectiveStartup.IsChecked = App.Config.CHECKBOX_LAST_GAME;
        RadioButtonDisableTitle.IsChecked = App.Config.CHECKBOX_TITLE;

        switch (App.Config.EXIT_MODE)
        {
            default:
                HoyoMessageBox.Show("ERROR", "ExitMode value is invalid,\ronly accepts \"1\" and \"2\"\rDefaulting to 1", HoyoSettingStatic);
                RadioButtonToTray.IsChecked = true;
                App.Config.EXIT_MODE = 1;
                App.Config.SaveConfig();
                break;
            case 1: RadioButtonToTray.IsChecked = true; break;
            case 2: RadioButtonToExit.IsChecked = true; break;
        }
        
        switch (App.Config.SCALING)
        {
            default:
                HoyoMessageBox.Show("ERROR", "WindowScale value is invalid,\ronly accepts \"1.0\" to \"1.3\"\rDefaulting to 1.0", HoyoSettingStatic);
                RadioButtonScale_1x.IsChecked = true;
                App.Config.SCALING = 1.0D;
                App.Config.SaveConfig();
                break;
            case 1.0D: RadioButtonScale_1x.IsChecked = true; break;
            case 1.1D: RadioButtonScale_2x.IsChecked = true; break;
            case 1.2D: RadioButtonScale_3x.IsChecked = true; break;
            case 1.3D: RadioButtonScale_4x.IsChecked = true; break;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        HoyoWindow.BLACK_THING.Visibility = Visibility.Collapsed;
        base.OnClosed(e);
    }

    void LocationButtonClick(object s, RoutedEventArgs e)
    {
        using var Folder = new Forms.FolderBrowserDialog();
        string path = null;
        e.Handled = true;

        if (Folder.ShowDialog() is Forms.DialogResult.Cancel)
            return;

        path = Folder.SelectedPath;

        switch(((Button)e.Source).Name)
        {
            case "GI_LOCATE": App.Config.GI_DIR = GI_DIR_TXT.Text = path; break;
            case "HSR_LOCATE": App.Config.HSR_DIR = HSR_DIR_TXT.Text = path; break;
            case "HI3_LOCATE": App.Config.HI3_DIR = HI3_DIR_TXT.Text = path; break;
        }
    }

    void ConfirmClick(object s, RoutedEventArgs e)
    {
        bool ErrorOccured = false;

        List<(TextBox config, HoyoGames AbsoluteName)> GameConfigs = new()
        {
            (GI_DIR_TXT, HoyoGames.GenshinImpact),
            (HSR_DIR_TXT, HoyoGames.HonkaiStarRail),
            (HI3_DIR_TXT, HoyoGames.HonkaiImpactThird)
        };

        foreach(var (config, name) in CollectionsMarshal.AsSpan(GameConfigs))
        {
            HoyoMain.ValidateSettings(config.Text, name, out bool IsInvalidGame);

            if(IsInvalidGame)
            {
                HoyoMessageBox.Show("❌ ERROR ❌", $"The \"{name.GAME_NAME}\" config cannot be found!\n or its an incorrect game.",HoyoSettingStatic);
                ErrorOccured = true;
            }
        }

        App.Config.SaveConfig();
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

    static void RefreshCurrentSelectedGame()
    {
        if(HoyoMain.CurrentGameSelected != HoyoGames.DEFAULT)
            GameChange.SetGame(--App.Config.LAST_GAME);
    }

    public static void ShowSettings(Window _w) =>
        new HoyoSettings() { Owner = _w }.ShowDialog();
}