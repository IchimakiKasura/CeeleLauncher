namespace HoyoLauncher.Core.API;

public sealed class RetrieveFile
{
    public static bool IsDownloading { get; set; } = false;
    public static bool IsPaused { get; set; } = false;

    static string FileLocation;
    public static async void DownloadFile(Uri link)
    {
        IsDownloading = true;

        HoyoWindow.VERSION_BUBBLE.Visibility = Visibility.Collapsed;
        HoyoWindow.ProgressBarElement.Visibility = Visibility.Visible;

        FileLocation = Path.Combine(HoyoMain.CurrentGameSelected.GAME_INSTALL_PATH, Path.GetFileName(link.LocalPath));

        using HttpClient request = new(handler: new HttpClientHandler() { Proxy = null })
        {
            Timeout = TimeSpan.FromDays(1),
            DefaultRequestHeaders =
            {
                CacheControl = new()
                {
                    NoCache = true
                }
            }
        };

        using var response = await request.GetAsync(link, HttpCompletionOption.ResponseHeadersRead);

        await Progress(response);
    }

    static async Task Progress(HttpResponseMessage response)
    {
        var totalBytesRead = 0L;
        var readCount = 0L;
        var buffer = new byte[4096];
        var IsComplete = false;
        var LastByte = 0L;
        var speed = 0;

        response.EnsureSuccessStatusCode();

        var TotalBytes = response.Content.Headers.ContentLength;

        using Stream stream = await response.Content.ReadAsStreamAsync();
        using FileStream fileStream = new(FileLocation, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, true);

        while(!IsComplete)
        {
            if (speed != DateTime.Now.Second)
            {
                double TotalByteSpeed = totalBytesRead - LastByte;
                speed = DateTime.Now.Second;
                LastByte = totalBytesRead;
                HoyoWindow.ProgressBarSpeed.Text = $"Bandwidth: {Math.Round(TotalByteSpeed / 1024 / 1024, 2)} MB/s";
            }

            if (IsPaused)
            {
                HoyoWindow.ProgressBarStatus.Text = $"Downloaded: {Math.Round((double)totalBytesRead / 1024 / 1024 / 1024, 2)}GB    |   {Math.Round((double)TotalBytes / 1024 / 1024 / 1024, 2)}GB | {Math.Round((double)totalBytesRead / TotalBytes.Value * 100, 2)}% (PAUSED)";
                await Task.Run(async () => { while (IsPaused) await Task.Delay(1); });
            }
            else
            {
                var bytesRead = await stream.ReadAsync(buffer);

                if (bytesRead is 0)
                {
                    IsComplete = true;
                    var percentage = Math.Round((double)totalBytesRead / TotalBytes.Value * 100, 2);
                    SetProgressBarValue(percentage);
                    UpdateProgress(totalBytesRead, TotalBytes.Value, percentage);
                    continue;
                }

                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                totalBytesRead += bytesRead;
                readCount += 1;

                if (readCount % 10 is 0)
                {
                    var percentage = Math.Round((double)totalBytesRead / TotalBytes.Value * 100, 2);
                    SetProgressBarValue(percentage);
                    UpdateProgress(totalBytesRead, TotalBytes.Value, percentage);
                }
            }
        }

        Finished();
    }

    static void UpdateProgress(in long totalBytesRead,in long TotalBytes,in double percent) =>
        HoyoWindow.ProgressBarStatus.Text = $"Downloaded: {Math.Round((double)totalBytesRead / 1024 / 1024 / 1024, 2)}GB    |   {Math.Round((double)TotalBytes / 1024 / 1024 / 1024, 2)}GB | {percent}%";

    static void Finished()
    {
        IsDownloading = false;
        HoyoWindow.LaunchButton.IsEnabled = 
        HoyoWindow.LaunchSelection.IsEnabled =
        HoyoWindow.HomeButton.IsEnabled = true;
        HoyoWindow.ProgressBarElement.Visibility = Visibility.Collapsed;
        HoyoWindow.LaunchButton.Content = HoyoMain.CurrentGameSelected.API_CACHE.DownloadFile is not null ? LaunchText.GAME_EXTRACT_TEXT : LaunchText.GAME_DEFAULT_TEXT;
    }

}