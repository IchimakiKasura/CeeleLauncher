using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;

namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoTooltips
{
    readonly static List<UIElement> Elements = new()
    {
        _hoyosettings.RadioButtonTray_Click,
        _hoyosettings.RadioButtonBackground_Click,
        _hoyosettings.RadioButtonSelectiveStartup_Click,
        _hoyosettings.Button_ClearLocationTexts,
        _hoyosettings.Button_ResetSettings,
        _hoyosettings.Funni
    };

    public static void SetToolTips()
    {
        foreach(var element in Elements)
        {
            element.MouseEnter += (s,e) => _hoyosettings.Tooltip_Text.Text = ((UIElement)s).Uid;
            element.MouseLeave += (s,e) => _hoyosettings.Tooltip_Text.Text = "";
        }
    }
}