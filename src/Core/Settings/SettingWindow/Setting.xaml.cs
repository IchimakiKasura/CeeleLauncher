using static define;

namespace HoyoLauncherProject.Core.SettingWindow;

public partial class Setting : Window
{
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
        var GameDirs = new List<(string dir, string name, string exec)>()
        {
            (GI_DIR_TXT.Text,  GENSHIN_IMPACT_TITLE,        GENSHIN_IMPACT_EXEC ),
            (HSR_DIR_TXT.Text, HONKAI_STAR_RAIL_TITLE,      HONKAI_STAR_RAIL_EXEC ),
            (HI3_DIR_TXT.Text, HONKAI_IMPACT_THIRD_TITLE,   HONKAI_IMPACT_THIRD_EXEC ),
            // (ZZZ_DIR_TXT.Text, "Zenless ZoneZero")
        };

        ValidateGameFiles(GameDirs, out bool ErrorOccured, out string AppName);

        AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR = GameDirs[GENSHIN_IMPACT_DIR].dir;
        AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR = GameDirs[HONKAI_STAR_RAIL_DIR].dir;
        AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR = GameDirs[HONKAI_IMPACT_THIRD_DIR].dir;
        // AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR = GameDirs[ZENLESS_ZONE_ZERO_DIR];

        HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR, Current.GI_DEFAULT);
        HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, Current.HSR_DEFAULT);
        HoyoLauncher.CheckGameDIRS(AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, Current.HI3_DEFAULT);

        AppLocal.HoyoLauncher.Default.Save();
        HoyoGames.Refresh();

        if (ErrorOccured)
        {
            MessageBox.Show($"ERROR:\n\nThe \"{AppName}\" location cannot be found!\n or its an incorrect game.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            switch(AppName)
            {
                case "Genshin Impact": Current.GI_DEFAULT.IsEnabled = false; break;
                case "Honkai Star Rail": Current.HSR_DEFAULT.IsEnabled = false; break;
                case "Honkai Impact 3rd": Current.HI3_DEFAULT.IsEnabled = false; break;
            }
            return;
        }

        Close();
    }

    public static void ValidateGameFiles(List<(string Directory, string Name, string Executable)> GameDirs, out bool isErrorOccured, out string AppName)
    {
        isErrorOccured = false;
        AppName = "";

        foreach(var (Dir, Name, Exec) in GameDirs)
        {
            AppName = Name;

            if (string.IsNullOrEmpty(Dir))
                break;
            
            if(!isErrorOccured && !GameConfig.IsConfigExist(Dir))
                isErrorOccured = true;

            if(!isErrorOccured && !GameConfig.Read(Dir).GameExecutable.Contains(Exec))
                isErrorOccured = true;

            if (isErrorOccured)
                break;
        }
    }

    private void GithubLink(object s, MouseButtonEventArgs e) =>
        Process.Start(new ProcessStartInfo()
        { 
            FileName = "https://github.com/IchimakiKasura/HoyoLauncher",
            UseShellExecute = true
        }).Dispose();
    
}
