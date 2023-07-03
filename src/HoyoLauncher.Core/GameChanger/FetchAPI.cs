namespace HoyoLauncher.Core.GameChanger;

public sealed partial class GameChange : HoyoMain
{
    static async Task<HoyoValues> FetchAPI(GameConfigRead GameConfig, HoyoValues values, ImageBrush GameBG)
    {
        HoyoWindow.VERSION_TEXT.Foreground = Brushes.Black;
        HoyoWindow.VERSION_TEXT.FontWeight = FontWeights.SemiBold;

        // Fetch once and never fetch again (until app was restarted)
        var GameAPI = CurrentGameSelected.API_CACHE ??= await RetrieveAPI.Fetch(CurrentGameSelected.GAME_CONTENT_API_LINK, CurrentGameSelected.GAME_RESOURCE_API_LINK);

        if (GameConfig.GameVersion != GameAPI.LatestVersion)
            VersionCheck(ref values, GameAPI.LatestVersion);

        if (GameConfig.GameBackgroundMD5 != GameAPI.BackgroundHASH)
            BacgroundHashCheck(ref values, ref GameAPI, GameBG);

        if (GameAPI.LatestVersion == "CONNECTION FAILURE, PLEASE RETRY AGAIN")
            APIFailure(ref values, GameBG);

        if(GameAPI is { PreDownloadFile: not null })
            PreDownloadCheck(ref values);

        if (GameAPI is { DownloadFile: not null } && File.Exists(Path.Combine(CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(GameAPI.DownloadFile.LocalPath))))
        {
            values.VersionBubble = Visibility.Collapsed;
            values.LaunchButtonContent = LaunchText.GAME_EXTRACT_TEXT;
        }

        return values;
    }

    static void VersionCheck(ref HoyoValues values, string LatestVersion)
    {
        HoyoWindow.VERSION_TEXT.Text = LatestVersion;

        ExecutableName = Path.Combine(CurrentGameSelected.GAME_DIRECTORY, "launcher.exe");

        values.VersionBubble = Visibility.Visible;
        values.LaunchButtonContent = LaunchText.GAME_UPDATE_TEXT;
    }

    static void BacgroundHashCheck(ref HoyoValues values, ref RetrieveAPI GameAPI, ImageBrush GameBG)
    {
        values.Background = GameAPI.BackgroundLINK ??= GameBG;

        if (CurrentGameSelected is { API_CACHE: not null })
            HoyoMessageBox.Show(HoyoWindow.Title, "Its Recommended that you open the Original Launcher To fetch its Background permanently.", HoyoWindow);
    }

    static void APIFailure(ref HoyoValues values, ImageBrush GameBG)
    {
        ConnectionFailure(ref values, GameBG);
        values.VersionBubble = Visibility.Visible;
        values.LaunchButton = false;
        HoyoWindow.VERSION_TEXT.Foreground = Brushes.Red;
        HoyoWindow.VERSION_TEXT.FontWeight = FontWeights.Bold;
        CurrentGameSelected.API_CACHE = null;
    }

    static void PreDownloadCheck(ref HoyoValues values)
    {
        values.VersionBubble = Visibility.Collapsed;
        values.PreInstall = Visibility.Visible;
    }
}