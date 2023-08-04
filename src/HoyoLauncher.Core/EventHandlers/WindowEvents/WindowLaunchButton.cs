namespace HoyoLauncher.Core.EventHandlers.WindowEvents;

[Events]
public sealed class WindowLaunchButton
{
    public static void Method() =>
        HoyoWindow.LaunchButton.Click += EventClick;

    private static async void EventClick(object s, RoutedEventArgs e)
    {
        e.Handled = true;

        if(HoyoMain.CurrentGameSelected == HoyoGames.TearsOfThemis)
        {
            HoyoMain.ProcessStart(HoyoMain.CurrentGameSelected.GAME_DIRECTORY);
            return;
        }

        if(!Equals(HoyoWindow.LaunchButton.Content, LaunchText.GAME_DEFAULT_TEXT))
        {
            if(Directory.GetFiles(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, "*.*").Where(s=>s.EndsWith(".zip") || s.EndsWith(".7z")).Any())
            {
                HoyoMain.ProcessStart(HoyoMain.ExecutableName);
                HoyoMessageBox.Show("A Very Cool Message Box", "Opening Original Launcher to Update the game!\r\rIf the File is downloaded, You can just press the Update again on the Original Launcher\rAnd it will extract it smoothly.\r\r If the File was stopped mid-way of downloading, It will resume its progress on the Original Launcher.\r", HoyoWindow);
            }
            else
            {
                HoyoWindow.HomeButton.IsEnabled =
                HoyoWindow.LaunchSelection.IsEnabled =
                HoyoWindow.LaunchButton.IsEnabled = false;
                HoyoWindow.LaunchButton.Content = "Downloading";
                RetrieveFile.DownloadFile(HoyoMain.CurrentGameSelected.API_CACHE.DownloadFile);
            }

            return;
        }

        HoyoWindow.GameSelection.Visibility = Visibility.Hidden;

        App.AppMinimizeToTray();
        HoyoMain.IsGameRunning = true;

        using Process GameProcess = Process.Start(
            new ProcessStartInfo()
            {
                FileName = HoyoMain.ExecutableName,
                UseShellExecute = true
            }
        );

        await GameProcess.WaitForExitAsync();
        HoyoMain.IsGameRunning = false;

        if(HoyoWindow.WindowState is WindowState.Minimized)
        {
            HoyoWindow.WindowState = WindowState.Normal;
            HoyoWindow.Show();
            HoyoWindow.ShowInTaskbar = true;
            App.AppTray.Visible = false;
        }

        HoyoWindow.Activate();
    }
}