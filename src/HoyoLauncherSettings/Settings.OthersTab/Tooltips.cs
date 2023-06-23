namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoTooltips
{
    public static void SetToolTips()
    {
        List<UIElement> Elements = new()
        {
            HoyoSettingStatic.RadioButtonTray_Click,
            HoyoSettingStatic.RadioButtonBackground_Click,
            HoyoSettingStatic.RadioButtonSelectiveStartup_Click,
            HoyoSettingStatic.RadioButtonDisableTitle_Click,
            HoyoSettingStatic.Button_ClearLocationTexts,
            HoyoSettingStatic.Button_ResetSettings,
            HoyoSettingStatic.Funni
        };

        foreach(var element in Elements)
        {
            element.MouseEnter += (s,e) => HoyoSettingStatic.Tooltip_Text.Text = ((UIElement)s).Uid;
            element.MouseLeave += (s,e) => HoyoSettingStatic.Tooltip_Text.Text = "";
        }
    }
}