namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoTooltips
{
    public static void SetToolTips()
    {
        foreach (var Elements in HoyoSettingStatic.GetType().GetFields(HoyoSettingStatic._flags))
            if (Elements.GetValue(HoyoSettingStatic) is UIElement ElementToolTip && ElementToolTip.Uid is not "")
            {
                ElementToolTip.MouseEnter += (s,e) => HoyoSettingStatic.Tooltip_Text.Text = ElementToolTip.Uid;
                ElementToolTip.MouseLeave += (s,e) => HoyoSettingStatic.Tooltip_Text.Text = "";
            }
    }
}