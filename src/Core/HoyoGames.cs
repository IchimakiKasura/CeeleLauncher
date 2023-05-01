namespace HoyoLauncherProject.Core;

public sealed class HoyoGames
{
    public static string GenshinImpact { get => Settings.HoyoLauncher.Default.GENSHIN_IMPACT_DIR; }
    public static string HonkaiStarRail { get => Settings.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR; }
    public static string HonkaiImpact3RD { get => Settings.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR; }
    public static string ZenlessZoneZero { get => Settings.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR; }
}
