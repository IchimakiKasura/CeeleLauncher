namespace HoyoLauncherProject.Core.SettingWindow;

public partial class Setting : Window
{
    const int
    GENSHIN_IMPACT_DIR = 0,
    HONKAI_STAR_RAIL_DIR = 1,
    HONKAI_IMPACT_THIRD_DIR = 2,
    ZENLESS_ZONE_ZERO_DIR = 3;

    public Setting()
    {
        InitializeComponent();

        Current.BLACK_THING.Opacity = 0.5;

        ExitButton.Click += (s,e)=>Close();
        CancelButton.Click += (s,e)=>Close();
        ConfirmButton.Click += ConfirmButtonClick;

        SettingDrag.MouseDown +=(s,e)=>DragMove();

        GI_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR;
        HSR_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR;
        HI3_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR;
        ZZZ_DIR_TXT.Text = AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR;
    }

    protected override void OnClosed(EventArgs e)
    {
        Current.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }

    private void ConfirmButtonClick(object s, RoutedEventArgs e)
    {
        bool ErrorOccured = false;
        string NameofGameError = "";
        var GameDirs = new System.Collections.Generic.List<(string dir, string name, string exec)>()
        {
            (GI_DIR_TXT.Text, "Genshin Impact", "GenshinImpact.exe"),
            (HSR_DIR_TXT.Text, "Honkai Star Rail", "StarRail.exe"),
            (HI3_DIR_TXT.Text, "Honkai Impact 3rd", "BH3.exe"),
            // (ZZZ_DIR_TXT.Text, "Zenless ZoneZero")
        };

        foreach(var (dir, name, exec) in GameDirs)
        {
            NameofGameError = name;

            if(!string.IsNullOrEmpty(dir))
            {
                if(!GameConfig.IsConfigExist(dir))
                {
                    ErrorOccured = true;
                    break;
                }
                
                var Config = GameConfig.Read(dir);
                
                if(!Config.GameExecutable.Contains(exec))
                {
                    ErrorOccured = true;
                    break;
                }
            }

        }

        AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR = GameDirs[GENSHIN_IMPACT_DIR].dir;
        AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR = GameDirs[HONKAI_STAR_RAIL_DIR].dir;
        AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR = GameDirs[HONKAI_IMPACT_THIRD_DIR].dir;
        // AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR = GameDirs[ZENLESS_ZONE_ZERO_DIR];

        HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR, Current.GI_DEFAULT);
        HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, Current.HSR_DEFAULT);
        HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, Current.HI3_DEFAULT);

        AppLocal.HoyoLauncher.Default.Save();
        HoyoGames.Refresh();

        if(ErrorOccured)
        {
            MessageBox.Show($"ERROR:\n\nThe \"{NameofGameError}\" location cannot be found!\n or its an incorrect game.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            switch(NameofGameError)
            {
                case "Genshin Impact": Current.GI_DEFAULT.IsEnabled = false; break;
                case "Honkai Star Rail": Current.HSR_DEFAULT.IsEnabled = false; break;
                case "Honkai Impact 3rd": Current.HI3_DEFAULT.IsEnabled = false; break;
            }
            return;
        }

        Close();
    }
}
