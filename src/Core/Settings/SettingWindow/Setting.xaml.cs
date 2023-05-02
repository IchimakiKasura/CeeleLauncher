namespace HoyoLauncherProject.Core.SettingWindow;

public partial class Setting : Window
{
    public Setting()
    {
        InitializeComponent();

        Current.BLACK_THING.Opacity = 0.5;

        ExitButton.Click += (s, e) => Close();
        CancelButton.Click += (s, e) => Close();

        ConfirmButton.Click += (s, e) =>
        {
            AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR = GI_DIR_TXT.Text;
            AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR = HSR_DIR_TXT.Text;
            AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR = HI3_DIR_TXT.Text;
            AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR = ZZZ_DIR_TXT.Text;

            HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR, Current.GI_DEFAULT);
            HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, Current.HSR_DEFAULT);
            HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, Current.HI3_DEFAULT);

            AppLocal.HoyoLauncher.Default.Save();
            HoyoGames.Refresh();
            
            Close();
        };

        SettingDrag.MouseDown += (s, e) => DragMove();

        GI_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR;
        HSR_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR;
        HI3_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR;
        ZZZ_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR;
    }

    protected override void OnClosed(EventArgs e)
    {
        MainWindow.Current.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }
}
