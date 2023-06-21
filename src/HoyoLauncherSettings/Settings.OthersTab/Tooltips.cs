using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;

namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoTooltips
{
    static List<UIElement> Elements;
    public static void SetToolTips()
    {
        Elements = new()
        {
            _hoyosettings.RadioButtonTray_Click,
            _hoyosettings.RadioButtonBackground_Click,
            _hoyosettings.RadioButtonSelectiveStartup_Click,
            _hoyosettings.RadioButtonDisableTitle_Click,
            _hoyosettings.Button_ClearLocationTexts,
            _hoyosettings.Button_ResetSettings,
            _hoyosettings.Funni
        };
        foreach(var element in Elements)
        {
            element.MouseEnter += (s,e) => _hoyosettings.Tooltip_Text.Text = ((UIElement)s).Uid;
            element.MouseLeave += (s,e) => _hoyosettings.Tooltip_Text.Text = "";
        }
    }
}