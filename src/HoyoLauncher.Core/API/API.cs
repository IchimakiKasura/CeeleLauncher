using System.Net;
using System.Text;
using System.Text.Json;

namespace HoyoLauncher.Core.API;

public sealed class RetrieveAPI
{
    public string LatestVersion { get; set; }
    public string BackgroundHASH { get; set; }
    public ImageBrush BackgroundLINK { get; set; }

    readonly JsonElement Resources;
    readonly JsonElement Content;

    public RetrieveAPI(string CONTENT, string RESOURCES)
    {
        using Stream ContentStreamData = CheckVersion(CONTENT).Result;
        using Stream ResourcesStreamData = CheckVersion(RESOURCES).Result;

        if (ContentStreamData == Stream.Null || ResourcesStreamData == Stream.Null) return;

        using StreamReader ContentReader = new(ContentStreamData, Encoding.UTF8);
        using StreamReader ResourcesReader = new(ResourcesStreamData, Encoding.UTF8);
        
        Content = JsonDocument.Parse(ContentReader.ReadToEnd()).RootElement;
        Resources = JsonDocument.Parse(ResourcesReader.ReadToEnd()).RootElement;

        SetAPIValues();
    }

    void SetAPIValues() 
    {
        try
        {
            LatestVersion = Resources.GetProperty("data").GetProperty("game").GetProperty("latest").GetProperty("version").ToString();

            BackgroundHASH = Content.GetProperty("data").GetProperty("adv").GetProperty("bg_checksum").ToString();
            BackgroundLINK = 
                new ImageBrush(new BitmapImage(new(Content.GetProperty("data").GetProperty("adv").GetProperty("background").ToString())));
        }
        catch {
            LatestVersion = "CONNECTION FAILURE, PLEASE RETRY AGAIN";
            BackgroundHASH = "";
            BackgroundLINK = null;
        }
    }

    static async Task<Stream> CheckVersion(string APILink)
    {
        HttpResponseMessage resp;

        try
        {
            using (HttpClient req = new(handler: new HttpClientHandler() { Proxy = null }))
            {
                req.Timeout = TimeSpan.FromMilliseconds(800);
                req.DefaultRequestVersion = HttpVersion.Version30;
                req.DefaultRequestHeaders.Add("User-Agent", "VersionCheck");
                var res = req.GetAsync(APILink).Result;
                resp = res;
            }

            return await resp.Content.ReadAsStreamAsync();
        }
        catch { return Stream.Null; }
    }
}