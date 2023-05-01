namespace HoyoLauncherProject.Core;

public sealed class HoyoGames
{
    public string DIR { get; }
    public string URL { get; }

    private HoyoGames(string str, string url)
    {
        DIR = str;
        URL = url;
    }

    public readonly static HoyoGames DEFAULT = new("", "https://www.hoyoverse.com/en-us/");
    public readonly static HoyoGames GenshinImpact = new(AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR, "https://genshin.hoyoverse.com/en");
    public readonly static HoyoGames HonkaiStarRail = new(AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR, "https://hsr.hoyoverse.com/en-us/");
    public readonly static HoyoGames HonkaiImpact3RD = new(AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR, "https://honkaiimpact3.hoyoverse.com/global/en-us/fab");
    public readonly static HoyoGames ZenlessZoneZero = new(AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR, "");

}