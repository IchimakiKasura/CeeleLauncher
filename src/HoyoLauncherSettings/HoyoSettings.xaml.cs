global using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;
using HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;

namespace HoyoLauncher.HoyoLauncherSettings;

// FIXME: refactor this
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

        // So it syncs when setting window is open, the main window gets grayed out at the same time.
        Loaded += (s,e) => HoyoWindow.BLACK_THING.Visibility = Visibility.Visible;
    }
    
    protected override void OnClosed(EventArgs e)
    {
        HoyoWindow.BLACK_THING.Visibility = Visibility.Collapsed;
        base.OnClosed(e);
    }

    static void RefreshCurrentSelectedGame()
    {
        if(HoyoMain.CurrentGameSelected != HoyoGames.DEFAULT)
            GameChange.SetGame(--App.Config.LAST_GAME);
    }

    public static void ShowSettings(Window Owner) =>
        new HoyoSettings() { Owner = Owner }.ShowDialog();
}