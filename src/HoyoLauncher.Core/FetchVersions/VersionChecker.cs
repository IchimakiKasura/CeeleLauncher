using System.Net;
using System.Text;
using System.Text.Json;

namespace HoyoLauncher.Core.FetchVersions;

sealed class VersionChecker
{
    readonly JsonElement Serialized;

    public VersionChecker(string API_LINK)
    {
        using Stream streamData = CheckVersion(API_LINK).Result;

        if (streamData == Stream.Null)
            return;

        using StreamReader reader = new(streamData, Encoding.UTF8);
        string Data = reader.ReadToEnd();

        Serialized = JsonDocument.Parse(Data).RootElement;
    }

    public string Version() =>
        Serialized.GetProperty("data").GetProperty("game").GetProperty("latest").GetProperty("version").ToString();

    public APIBACKGROUND BackgroundHash() =>
        new()
        {
            HASH = Serialized.GetProperty("data").GetProperty("adv").GetProperty("bg_checksum").ToString(),
            BG_LINK = Serialized.GetProperty("data").GetProperty("adv").GetProperty("background").ToString()
        };

    static async Task<Stream> CheckVersion(string APILink)
    {
        HttpResponseMessage resp;

        try
        {
            using (HttpClient req = new())
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

sealed class APIBACKGROUND
{
    public string HASH { get; set; }
    public string BG_LINK { get; set; }
}
