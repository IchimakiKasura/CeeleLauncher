namespace HoyoLauncher.Core.EventHandlers;

[Events]
public sealed class Events__ : IEvents
{
    public static void Method() { }

    public static void GameSelectionBackgroundSet()
    {
        HoyoWindow.GAME_SELECTION_GI.Background = SelectionResources.GenshinImage;
        HoyoWindow.GAME_SELECTION_HSR.Background = SelectionResources.HonkaiStarRailImage;
        HoyoWindow.GAME_SELECTION_HI3.Background = SelectionResources.HonkaiImpactImage;
        HoyoWindow.GAME_SELECTION_ZZZ.Background = SelectionResources.ZZZImage;
        HoyoWindow.GAME_SELECTION_TOT.Background = SelectionResources.TOTImage;
    }

    public static void DownloadPauseButton()
    {
        Geometry Paused = Geometry.Parse("M8 6 L8 22 L13 22 L13 6 Z M17 6 L17 22 L22 22 L22 6 Z");
        Geometry Play = Geometry.Parse("M10 6 L10 22 L22 14 Z");

        HoyoWindow.ProgressBarButton.MouseDown += (s, e) =>
        {
            if (e.ChangedButton is not MouseButton.Left) return;

            RetrieveFile.IsPaused = !RetrieveFile.IsPaused;

            HoyoWindow.PathPause.Data = RetrieveFile.IsPaused ? Play : Paused;
        };

        HoyoWindow.ProgressBarButton.MouseEnter += (s, e) =>
            (e.Source as Border).Background = App.ConvertColorFromString("#9FFFFFFF");

        HoyoWindow.ProgressBarButton.MouseLeave += (s, e) =>
            (e.Source as Border).Background = App.ConvertColorFromString("#3FFFFFFF");
    }
    
    public static void PreDownloadButton()
    {
        HoyoWindow.PreDownloadButton.MouseDown += (s,e) =>
        {
            if(e.ChangedButton is not MouseButton.Left) return;

            if(File.Exists(Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(HoyoMain.CurrentGameSelected.API_CACHE.PreDownloadFile.LocalPath))))
            {
                HoyoMessageBox.Show(HoyoWindow.Title, "The file is already downloaded!\r\rIf the download was stopped last time, It need to be re-downloaded again by deleting the file.\r", HoyoWindow);
                return;
            }

            HoyoWindow.HomeButton.IsEnabled =
            HoyoWindow.LaunchSelection.IsEnabled =
            HoyoWindow.LaunchButton.IsEnabled = false;
            HoyoWindow.LaunchButton.Content = "Downloading";
            HoyoWindow.PreDownload.Visibility = Visibility.Collapsed;
            RetrieveFile.DownloadFile(HoyoMain.CurrentGameSelected.API_CACHE.PreDownloadFile);
        };

        HoyoWindow.PreDownloadButton.MouseEnter += (s, e) => HoyoWindow.PreDownloadButton.Opacity = 0.5;
        HoyoWindow.PreDownloadButton.MouseLeave += (s, e) => HoyoWindow.PreDownloadButton.Opacity = 1;
    }
}