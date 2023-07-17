namespace HoyoLauncher.Core.EventHandlers.WindowEvents;

[Events]
public sealed class WindowToolTips
{
    public static void Method()
    {
        HoyoWindow.GENSHIN_IMPACT_REWARDS.ButtonToolTip = ToolTips.GENSHIN_IMPACT_TIP;
        HoyoWindow.HONKAI_STAR_RAIL_REWARDS.ButtonToolTip = ToolTips.HONKAI_STAR_RAIL_TIP;
        HoyoWindow.HONKAI_IMPACT_THIRD_REWARDS.ButtonToolTip = ToolTips.HONKAI_IMPACT_THIRD_TIP;
        HoyoWindow.ZZZ_REWARDS.ButtonToolTip = ToolTips.ZZZ_TIP;
        HoyoWindow.TOT_SITE.ButtonToolTip = ToolTips.TOT_TIP;
        HoyoWindow.GameHomePage.ButtonToolTip = ToolTips.HOMEPAGE_TIP;
        HoyoWindow.GameMapPage.ButtonToolTip = ToolTips.MAPPAGE_TIP;
        HoyoWindow.GameScreenshotFolder.ButtonToolTip = ToolTips.SCREENSHOT_TIP;
        HoyoWindow.GameOriginalLauncher.ButtonToolTip = ToolTips.LAUNCHER_TIP;
        HoyoWindow.SettingsButton.ButtonToolTip = ToolTips.TOPBUTTON_SETTINGS_TIP;
        HoyoWindow.HomeButton.ButtonToolTip = ToolTips.TOPBUTTON_HOME_TIP;
        HoyoWindow.RefreshButton.ButtonToolTip = ToolTips.TOPBUTTON_REFRESH_TIP;
    }
}