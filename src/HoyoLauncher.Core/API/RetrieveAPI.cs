namespace HoyoLauncher.Core.API;

public sealed class RetrieveAPI
{
    public string LatestVersion { get; private set; } = "CONNECTION FAILURE, PLEASE RETRY AGAIN";
    public string BackgroundHASH { get; private set; } = "timeout";
    public Uri DownloadFile { get; private set; }
    public Uri PreDownloadFile { get; private set; }
    public ImageBrush BackgroundLINK { get; set; }

    readonly ObjectList Resources, Content;

    public static async Task<RetrieveAPI> Fetch(string CONTENT, string RESOURCES)
    {
        using Stream
        ContentStreamData = await CheckVersion(CONTENT),
        ResourcesStreamData = await CheckVersion(RESOURCES);

        return new(ReadJsonData(ContentStreamData), ReadJsonData(ResourcesStreamData));
    }

    private RetrieveAPI(ObjectList _Content, ObjectList _Resources)
    {   
        Content = _Content;
        Resources = _Resources;
        SetAPIValues();
    }
    
    void SetAPIValues() 
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
                Latest Version      :     {{LatestVersion}}
                
                Download File Link  :     {{DownloadFile?.ToString() ?? "EMPTY"}}
                Pre Installation    :     {{PreDownloadFile?.ToString() ?? "EMPTY"}}

                Background Link     :     {{BackgroundLINK?.ImageSource?.ToString() ?? "EMPTY"}}
                Background Hash     :     {{BackgroundHASH.ToString() ?? "EMPTY"}}
            }
            """);
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    static ObjectList ReadJsonData(in Stream stream)
    {
        using StreamReader DataStream = new(stream, Encoding.UTF8);
        string DataJSON = stream != Stream.Null ? DataStream.ReadToEnd() : "{}";
        return JsonSerializer.Deserialize<ObjectList>(DataJSON);
    }
    
    static async Task<Stream> CheckVersion(string APILink)
    {
        HttpResponseMessage resp;

        using HttpClient req = new(handler: new HttpClientHandler() { Proxy = null })
        {
            Timeout = TimeSpan.FromSeconds(10),
            DefaultRequestHeaders =
            {
                CacheControl = new()
                {
                    NoCache = true,
                }
            }
        };

        try
        {
            resp = await req.GetAsync(APILink, HttpCompletionOption.ResponseHeadersRead);

            resp.EnsureSuccessStatusCode();

            if(resp.IsSuccessStatusCode)
                return await resp.Content.ReadAsStreamAsync();
        }
        catch (TaskCanceledException)
        {
            Debug.WriteLine("Connection timeout! Slow Connection detected");
        }
        catch (HttpRequestException)
        {
            Debug.WriteLine("Connection timeout! No Internet Connection");
        }

        return Stream.Null;
    }
}