﻿namespace HoyoLauncher.HoyoLauncher_Core;

public sealed class HoyoGames
{
    public string GAME_DIRECTORY { get; private set; }
    public string GAME_EXECUTABLE { get; }
    public string GAME_NAME { get; }
    public string GAME_HOMEPAGE { get; }
    public string GAME_CHECK_IN_PAGE { get; }

    private HoyoGames(
        string GameDirectory,
        string GameExecutable,
        string GameName,
        string GameHomepage,
        string GameCheckInPage
        )
    {
        GAME_DIRECTORY = GameDirectory;
        GAME_EXECUTABLE = GameExecutable;
        GAME_NAME = GameName;
        GAME_HOMEPAGE = GameHomepage;
        GAME_CHECK_IN_PAGE = GameCheckInPage;
    }

    public readonly static HoyoGames DEFAULT =
        new("","", "", "https://www.hoyoverse.com/en-us/","");

    public readonly static HoyoGames GenshinImpact =
        new
        (
            AppSettings.Settings.Default.GENSHIN_IMPACT_DIR,
            AppResources.Resources.GENSHIN_IMPACT_EXEC,
            AppResources.Resources.GENSHIN_IMPACT_TITLE,
            "https://genshin.hoyoverse.com/en",
            "https://act.hoyolab.com/ys/event/signin-sea-v3/index.html?act_id=e202102251931481&hyl_auth_required=true&hyl_presentation_style=fullscreen&utm_source=hoyolab&utm_medium=tools&lang=en-us&bbs_theme=dark&bbs_theme_device=1"
        );

    public readonly static HoyoGames HonkaiStarRail =
        new
        (
            AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR,
            AppResources.Resources.HONKAI_STAR_RAIL_EXEC,
            AppResources.Resources.HONKAI_STAR_RAIL_TITLE,
            "https://hsr.hoyoverse.com/en-us/",
            "https://act.hoyolab.com/bbs/event/signin/hkrpg/index.html?act_id=e202303301540311&hyl_auth_required=true&hyl_presentation_style=fullscreen&utm_source=hoyolab&utm_medium=tools&utm_campaign=checkin&utm_id=6&lang=en-us&bbs_theme=dark&bbs_theme_device=1"
        );

    public readonly static HoyoGames HonkaiImpactThird =
        new
        (
            AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR,
            AppResources.Resources.HONKAI_IMPACT_THIRD_EXEC,
            AppResources.Resources.HONKAI_IMPACT_THIRD_TITLE,
            "https://honkaiimpact3.hoyoverse.com/global/en-us/fab",
            "https://act.hoyolab.com/bbs/event/signin-bh3/index.html?act_id=e202110291205111&utm_source=hoyolab&utm_medium=tools&bbs_theme=dark&bbs_theme_device=1"
        );

    public readonly static HoyoGames ZenlessZoneZero =
        new
        (
            AppSettings.Settings.Default.ZENLESS_ZONE_ZERO_DIR,
            "",
            "",
            "https://zenless.hoyoverse.com/en-us",
            ""
        );

    public readonly static HoyoGames TearsOfThemis =
        new
        (
            "https://play.google.com/store/apps/details?id=com.miHoYo.tot.glb",
            "",
            "",
            "https://tot.hoyoverse.com/en-us",
            ""
        );

    public static void RefreshDirectory()
    {
        GenshinImpact.GAME_DIRECTORY = AppSettings.Settings.Default.GENSHIN_IMPACT_DIR;
        HonkaiStarRail.GAME_DIRECTORY = AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR;
        HonkaiImpactThird.GAME_DIRECTORY = AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR;
    }
}