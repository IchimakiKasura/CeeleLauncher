namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoRadioButtons
{
    public static void SetRadioButtons()
    {
        List<(UIElement el, string name)> MouseDownAction = new()
        {
            (HoyoSettingStatic.RadioButtonTray_Click, "CHECKBOX_EXIT_TRAY"),
            (HoyoSettingStatic.RadioButtonBackground_Click, "CHECKBOX_BACKGROUND"),
            (HoyoSettingStatic.RadioButtonSelectiveStartup_Click, "CHECKBOX_LAST_GAME"),
            (HoyoSettingStatic.RadioButtonDisableTitle_Click, "CHECKBOX_TITLE")
        };

        // CANT DO THE "REF" KEYWORD 
        foreach(var (Element, Name) in MouseDownAction)
            Element.MouseDown += (s,e) =>
            {
                if(e.ChangedButton is not MouseButton.Left) return;

                switch(Name)
                {
                    case "CHECKBOX_EXIT_TRAY": App.Config.CHECKBOX_EXIT_TRAY = (bool)(HoyoSettingStatic.RadioButtonTray.IsChecked = !HoyoSettingStatic.RadioButtonTray.IsChecked); break;
                    case "CHECKBOX_BACKGROUND": App.Config.CHECKBOX_BACKGROUND = (bool)(HoyoSettingStatic.RadioButtonBackground.IsChecked = !HoyoSettingStatic.RadioButtonBackground.IsChecked); break;
                    case "CHECKBOX_LAST_GAME": App.Config.CHECKBOX_LAST_GAME = (bool)(HoyoSettingStatic.RadioButtonSelectiveStartup.IsChecked = !HoyoSettingStatic.RadioButtonSelectiveStartup.IsChecked); break;
                    case "CHECKBOX_TITLE": App.Config.CHECKBOX_TITLE = (bool)(HoyoSettingStatic.RadioButtonDisableTitle.IsChecked = !HoyoSettingStatic.RadioButtonDisableTitle.IsChecked); break;
                }
            };
    }
}