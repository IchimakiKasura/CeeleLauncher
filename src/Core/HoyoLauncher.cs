namespace HoyoLauncherProject.Core;

sealed class HoyoLauncher 
{
    public static void Initialize()
    {
        EventHandlers.Initialize();

        // TEMPORARY
        // Settings.HoyoLauncher.Default.GENSHIN_IMPACT_DIR = "H:\\Genshin Impact";
        // Settings.HoyoLauncher.Default.HONKAI_STAR_RAIL_DIR = "H:\\Star Rail";
        // Settings.HoyoLauncher.Default.HONKAI_IMPACT_THIRD_DIR = "H:\\Honkai Impact 3rd";
        // Settings.HoyoLauncher.Default.ZENLESS_ZONE_ZERO_DIR = "H:\\Zenless Zone Zero";
    }
}