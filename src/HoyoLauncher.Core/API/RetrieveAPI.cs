namespace HoyoLauncher.Core.API;

public sealed class RetrieveAPI
{
    public string LatestVersion { get; private set; } = "CONNECTION FAILURE, PLEASE RETRY AGAIN";
    public string BackgroundHASH { get; private set; } = "";
    public Uri DownloadFile { get; private set; } = null;
    public Uri PreDownloadFile { get; private set; } = null;
    public ImageBrush BackgroundLINK { get; set; } = null;

    readonly JsonElement Resources;
    readonly JsonElement Content;

    public static async Task<RetrieveAPI> Fetch(string CONTENT, string RESOURCES)
    {
        using Stream
        ContentStreamData = await CheckVersion(CONTENT),
        ResourcesStreamData = await CheckVersion(RESOURCES);

        return new(ReadJsonData(ContentStreamData).RootElement, ReadJsonData(ResourcesStreamData).RootElement);
    }

    private RetrieveAPI(JsonElement _Content, JsonElement _Resources)
    {
        Content = _Content;
        Resources = _Resources;
        SetAPIValues();
    }
    
    void SetAPIValues() 
    {
        if(Resources.TryGetProperty("data", out JsonElement VersionProperty))
        {
            var Latest = VersionProperty.GetProperty("game").GetProperty("latest");
            LatestVersion = Latest.GetProperty("version").ToString();

            if (Latest.TryGetProperty("path", out JsonElement FilePath))
                DownloadFile = FilePath.ToString() is not "" ? new(FilePath.ToString()) : null;

            if(VersionProperty.TryGetProperty("pre_download_game", out JsonElement pre_download))
                PreDownloadFile = !string.IsNullOrEmpty(pre_download.ToString()) ? new(pre_download.GetProperty("diffs")[0].GetProperty("path").ToString()) : null;
            
        }

        if (Content.TryGetProperty("data", out JsonElement ContentProperty))
        {
            var adv = ContentProperty.GetProperty("adv");

            BackgroundHASH = adv.GetProperty("bg_checksum").ToString();
            BackgroundLINK = new ImageBrush(new BitmapImage(new(adv.GetProperty("background").ToString())));
        }
    }

    static JsonDocument ReadJsonData(Stream stream)
    {
        using StreamReader DataStream = new(stream, Encoding.UTF8);
        string DataJSON = stream != Stream.Null ? DataStream.ReadToEnd() : "{}";
        return JsonDocument.Parse(DataJSON);
    }
    
    static async Task<Stream> CheckVersion(string APILink)
    {
        HttpResponseMessage resp;

        using HttpClient req = new(handler: new HttpClientHandler() { Proxy = null });// { Timeout = TimeSpan.FromMilliseconds(1000) };

        resp = await req.GetAsync(APILink, HttpCompletionOption.ResponseHeadersRead);

        resp.EnsureSuccessStatusCode();

        if(resp.IsSuccessStatusCode)
            return await resp.Content.ReadAsStreamAsync();
        return Stream.Null;
    }
}