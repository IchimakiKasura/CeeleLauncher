using static define;
namespace HoyoLauncherProject.Core;

public sealed class HoyoGames
{
    public string DIR { get; private set; }
    public string CURRENT_GAME { get; }
    public string CURRENT_EXECUTABLE { get; } 
    public string URL { get; }
    public string CHECKIN_URL { get; }

    private HoyoGames(string str, string url, string checkin_url, string currentGame, string exec)
    {
        DIR = str;
        URL = url;
        CHECKIN_URL = checkin_url;
        CURRENT_GAME = currentGame;
        CURRENT_EXECUTABLE = exec;
    }

    public readonly static HoyoGames DEFAULT = new("", "https://www.hoyoverse.com/en-us/", "", "","");
    
    public readonly static HoyoGames GenshinImpact = new
    (
        AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR,
        "https://genshin.hoyoverse.com/en",
        "https://act.hoyolab.com/ys/event/signin-sea-v3/index.html?act_id=e202102251931481&hyl_auth_required=true&hyl_presentation_style=fullscreen&utm_source=hoyolab&utm_medium=tools&lang=en-us&bbs_theme=dark&bbs_theme_device=1",
        GENSHIN_IMPACT_TITLE,
        GENSHIN_IMPACT_EXEC
    );

    public readonly static HoyoGames HonkaiStarRail = new
    (
        AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR,
        "https://hsr.hoyoverse.com/en-us/",
        "https://act.hoyolab.com/bbs/event/signin/hkrpg/index.html?act_id=e202303301540311&hyl_auth_required=true&hyl_presentation_style=fullscreen&utm_source=hoyolab&utm_medium=tools&utm_campaign=checkin&utm_id=6&lang=en-us&bbs_theme=dark&bbs_theme_device=1",
        HONKAI_STAR_RAIL_TITLE,
        HONKAI_STAR_RAIL_EXEC
    );
    
    public readonly static HoyoGames HonkaiImpact3RD = new
    (
        AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR,
        "https://honkaiimpact3.hoyoverse.com/global/en-us/fab",
        "https://act.hoyolab.com/bbs/event/signin-bh3/index.html?act_id=e202110291205111&utm_source=hoyolab&utm_medium=tools&bbs_theme=dark&bbs_theme_device=1",
        HONKAI_IMPACT_THIRD_TITLE,
        HONKAI_IMPACT_THIRD_EXEC
    );
    
    public readonly static HoyoGames ZenlessZoneZero = new(
        AppLocal.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR,
        "",
        "",
        "",
        "");

    public static void Refresh()
    {
        GenshinImpact.DIR = AppLocal.HoyoLauncher.Default.GENSHIN_IMPACT_DIR;
        HonkaiStarRail.DIR = AppLocal.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR;
        HonkaiImpact3RD.DIR = AppLocal.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR;
    }

}