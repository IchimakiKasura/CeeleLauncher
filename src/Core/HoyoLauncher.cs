namespace HoyoLauncherProject.Core;

class HoyoLauncher 
{
    private static MainWindow _m;

    // Temporary
    /// <summary>
    /// If TRUE, Genshin is selected <br/>
    /// If FALSE, Star Rail is selected.
    /// </summary>
    public static bool IsGenshin = true;

    public static void Initialize(MainWindow m)
    {
        _m = m;

        if (IsGenshin)
            m.ChangeBG(TEMPORARY.GENSHIN_IMPACT_DIR);
        else m.ChangeBG(TEMPORARY.HONKAI_STAR_RAIL_DIR);
    }
}

