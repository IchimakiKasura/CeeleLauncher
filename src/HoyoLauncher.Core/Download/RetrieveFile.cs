namespace HoyoLauncher.Core.Download;

// EXPERIMENTAL
public sealed class RetrieveFile
{
    public static bool IsDownloading { get; set; } = false;
    public static bool IsPaused { get; set; } = false;

    static string FileLocation;
    public static async void DownloadFile()
    {
        IsDownloading = true;

        HoyoWindow.VERSION_BUBBLE.Visibility = Visibility.Collapsed;
        HoyoWindow.ProgressBarElement.Visibility = Visibility.Visible;

        var link = HoyoMain.CurrentGameSelected.API_CACHE.DownloadFile;
        FileLocation = Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(link.LocalPath));
        
        using HttpClient request = new(handler: new HttpClientHandler() { Proxy = null });

        using var response = await request.GetAsync(link, HttpCompletionOption.ResponseHeadersRead);

        await Progress(response);
    }

    // Sad, no download speed :<
    static async Task Progress(HttpResponseMessage response)
    {
        var totalBytesRead = 0L;
        var readCount = 0L;
        var buffer = new byte[8192];
        var IsComplete = false;

        response.EnsureSuccessStatusCode();

        var TotalBytes = response.Content.Headers.ContentLength;

        using Stream stream = await response.Content.ReadAsStreamAsync();
        using FileStream fileStream = new(FileLocation, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

        while(!IsComplete)
        {
            if(IsPaused)
                HoyoWindow.ProgressBarDownloaded.Text = $"Downloaded: {Math.Round((double)totalBytesRead / 1024 / 1024 / 1024, 2)}GB    |   {Math.Round((double)TotalBytes / 1024 / 1024 / 1024, 2)}GB  (PAUSED)";

            await Task.Run(async() => { while (IsPaused) await Task.Delay(1); });
            
            var bytesRead = await stream.ReadAsync(buffer);

            if (bytesRead is 0)
            {
                IsComplete = true;
                SetProgressBarValue(Math.Round((double)totalBytesRead / TotalBytes.Value * 100, 2));
                UpdateProgress(totalBytesRead, TotalBytes.Value);
                continue;
            }

            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));

            totalBytesRead += bytesRead;
            readCount += 1;

            if (readCount % 10 is 0)
            {
                SetProgressBarValue(Math.Round((double)totalBytesRead / TotalBytes.Value * 100, 2));
                UpdateProgress(totalBytesRead, TotalBytes.Value);
            }
        }

        IsDownloading = false;
        HoyoWindow.LaunchButton.IsEnabled = 
        HoyoWindow.LaunchSelection.IsEnabled =
        HoyoWindow.HomeButton.IsEnabled = true;
        HoyoWindow.ProgressBarElement.Visibility = Visibility.Collapsed;
        HoyoWindow.LaunchButton.Content = "Extract";
    }

    static void UpdateProgress(long totalBytesRead, long TotalBytes) =>
        HoyoWindow.ProgressBarDownloaded.Text = $"Downloaded: {Math.Round((double)totalBytesRead / 1024 / 1024 / 1024, 2)}GB    |   {Math.Round((double)TotalBytes / 1024 / 1024 / 1024, 2)}GB";

}