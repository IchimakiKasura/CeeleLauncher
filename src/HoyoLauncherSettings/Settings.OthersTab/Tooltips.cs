namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoTooltips
{
    public static void SetToolTips()
    {
        List<UIElement> Elements = new()
        {
            HoyoSettingStatic.RadioButtonToTray,
            HoyoSettingStatic.RadioButtonToExit,
            HoyoSettingStatic.RadioButtonBackground_Click,
            HoyoSettingStatic.RadioButtonSelectiveStartup_Click,
            HoyoSettingStatic.RadioButtonDisableTitle_Click,
            HoyoSettingStatic.RadioButtonScale_1x,
            HoyoSettingStatic.RadioButtonScale_2x,
            HoyoSettingStatic.RadioButtonScale_3x,
            HoyoSettingStatic.RadioButtonScale_4x,
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