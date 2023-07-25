namespace HoyoLauncher.Core.API;

public sealed class RetrieveAPI
{
    public string LatestVersion { get; private set; } = "CONNECTION FAILURE, PLEASE RETRY AGAIN";
    public string BackgroundHASH { get; private set; } = "timeout";
    public Uri DownloadFile { get; private set; }
    public Uri PreDownloadFile { get; private set; }
    public ImageBrush BackgroundLINK { get; set; }

    private RetrieveAPI() { }

    public static async Task<RetrieveAPI> Fetch(string CONTENT, string RESOURCES)
    {
        var ContentData = await FetchJSON(CONTENT);
        var ResourceData = await FetchJSON(RESOURCES);

        RetrieveAPI data = new();
        data.SetAPIValues(ContentData, ResourceData);

        // JsonDebug.TestJson(); // uncomment this to run.

        return data;
    }

    private void SetAPIValues(ObjectList Content, ObjectList Resources) 
    {
        if(Resources is { data: not null })
        {
            LatestVersion = Resources.GetLatestVersion;
            var game = Resources.data.game;

            DownloadFile = Resources.IsLatestPathEmpty ?
                new(game.diffs[0].path) : new(game.latest.path);
            
            if(Resources.IsPreInstallAvailable)
                PreDownloadFile = new(Resources.GetPreInstallation);
        }

        if(Content is { data: not null })
        {
            BackgroundHASH = Content.GetBackgroundHash;
            BackgroundLINK = new(new BitmapImage(new(Content.GetBackgroundLink)));
        }

        Debug.WriteLine($$"""

        API FETCH INFO [Game: {{HoyoMain.CurrentGameSelected.GAME_NAME}}]
        {
            Latest Version      :     {{LatestVersion}} (Current: {{HoyoMain.CurrentGameSelected.GAME_CONFIG_CACHE.GameVersion}})
            Download File Link  :     {{DownloadFile?.ToString() ?? "EMPTY"}}
            Pre Installation    :     {{PreDownloadFile?.ToString() ?? "EMPTY"}}
            Background Link     :     {{BackgroundLINK?.ImageSource?.ToString() ?? "EMPTY"}}
            Background Hash     :     {{BackgroundHASH.ToString() ?? "EMPTY"}}
        }
            
        """);
    }
    
    static async Task<ObjectList> FetchJSON(string APILink)
    {
        HttpResponseMessage resp;
        HttpClientHandler Handler = new()
        {
            Proxy = null,
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.GZip
        };

        using HttpClient req = new(Handler)
        {
            Timeout = TimeSpan.FromSeconds(10),
            DefaultRequestHeaders = { CacheControl = new() { NoCache = true, } }
        };

        try
        {
            resp = await req.GetAsync(APILink, HttpCompletionOption.ResponseHeadersRead);

            resp.EnsureSuccessStatusCode();

            if(resp.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ObjectList>(await resp.Content.ReadAsStreamAsync());
        }
        catch (Exception e)
        {
            switch(e)
            {
                case TaskCanceledException: Debug.WriteLine("Connection timeout! Slow Connection detected"); break;
                case HttpRequestException:  Debug.WriteLine("Connection timeout! No Internet Connection  "); break;
            }
        }

        return new ObjectList();
    }
}