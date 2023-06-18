using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;

namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoTooltips
{
    public static void SetToolTips()
    {
        _hoyosettings.RadioButtonTray_Click.MouseEnter += ShowTooltip;
        _hoyosettings.RadioButtonBackground_Click.MouseEnter += ShowTooltip;
        _hoyosettings.RadioButtonSelectiveStartup_Click.MouseEnter += ShowTooltip;

        _hoyosettings.Button_ClearLocationTexts.MouseEnter += ShowTooltip;
        _hoyosettings.Button_ResetSettings.MouseEnter += ShowTooltip;

        _hoyosettings.Funni.MouseEnter += ShowTooltip;


        _hoyosettings.RadioButtonTray_Click.MouseLeave += RemoveTooltip;
        _hoyosettings.RadioButtonBackground_Click.MouseLeave += RemoveTooltip;
        _hoyosettings.RadioButtonSelectiveStartup_Click.MouseLeave += RemoveTooltip;

        _hoyosettings.Button_ClearLocationTexts.MouseLeave += RemoveTooltip;
        _hoyosettings.Button_ResetSettings.MouseLeave += RemoveTooltip;

        _hoyosettings.Funni.MouseLeave += RemoveTooltip;
    }

    static void ShowTooltip(object s, MouseEventArgs e) =>
        _hoyosettings.Tooltip_Text.Text = ((UIElement)s).Uid;

    static void RemoveTooltip(object s, MouseEventArgs e) =>
        _hoyosettings.Tooltip_Text.Text = "";
    
}