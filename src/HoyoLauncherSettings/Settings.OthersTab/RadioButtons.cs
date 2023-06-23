namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoRadioButtons
{
    public static void SetRadioButtons()
    {
        List<(UIElement el, string name)> MouseDownAction = new()
        {
            (HoyoSettingStatic.RadioButtonTray_Click, "CHECKBOX_MINIMIZE_TRAY"),
            (HoyoSettingStatic.RadioButtonBackground_Click, "CHECKBOX_BACKGROUND"),
            (HoyoSettingStatic.RadioButtonSelectiveStartup_Click, "CHECKBOX_LASTGAME"),
            (HoyoSettingStatic.RadioButtonDisableTitle_Click, "CHECKBOX_TITLE")
        };

        foreach(var (Element, Name) in MouseDownAction)
            Element.MouseDown += (s,e) =>
            {
                if(e.ChangedButton is not MouseButton.Left) return;

                AppSettings.Settings.Default[Name] = "" switch
                {
                    _ when Name is "CHECKBOX_MINIMIZE_TRAY" => HoyoSettingStatic.RadioButtonTray.IsChecked = !HoyoSettingStatic.RadioButtonTray.IsChecked,
                    _ when Name is "CHECKBOX_BACKGROUND" => HoyoSettingStatic.RadioButtonBackground.IsChecked = !HoyoSettingStatic.RadioButtonBackground.IsChecked,
                    _ when Name is "CHECKBOX_LASTGAME" => HoyoSettingStatic.RadioButtonSelectiveStartup.IsChecked = !HoyoSettingStatic.RadioButtonSelectiveStartup.IsChecked, 
                    _ when Name is "CHECKBOX_TITLE" => HoyoSettingStatic.RadioButtonDisableTitle.IsChecked = !HoyoSettingStatic.RadioButtonDisableTitle.IsChecked,
                    _ => ""
                };
            };
    }
}