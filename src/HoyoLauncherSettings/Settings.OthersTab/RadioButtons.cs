namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoRadioButtons
{
    const string BACKGROUND = "CHECKBOX_BACKGROUND",
    LAST_GAME = "CHECKBOX_LAST_GAME",
    TITLE = "CHECKBOX_TITLE",
    Btn_Background = "RadioButtonBackground",
    Btn_Startup = "RadioButtonSelectiveStartup",
    Btn_Title = "RadioButtonDisableTitle";

    public static void SetRadioButtons()
    {
        List<(UIElement Element, string Name, string Buttons)> MouseDownAction = new()
        {
            (HoyoSettingStatic.RadioButtonBackground_Click, BACKGROUND, Btn_Background),
            (HoyoSettingStatic.RadioButtonSelectiveStartup_Click, LAST_GAME, Btn_Startup),
            (HoyoSettingStatic.RadioButtonDisableTitle_Click, TITLE, Btn_Title)
        };

        foreach(var (Element, Name, Buttons) in MouseDownAction)
            Element.MouseDown += (s,e) =>
                App.Config[Name] =
                    e.ChangedButton is MouseButton.Left ? HoyoSettingStatic[Buttons].IsChecked = !HoyoSettingStatic[Buttons].IsChecked : null;

        HoyoSettingStatic.RadioButtonToTray.Checked += (s, e) => App.Config.EXIT_MODE = 1;
        HoyoSettingStatic.RadioButtonToExit.Checked += (s, e) => App.Config.EXIT_MODE = 2;

        HoyoSettingStatic.RadioButtonScale_1x.Checked += (s, e) => App.Config.SCALING = 1.0D;
        HoyoSettingStatic.RadioButtonScale_2x.Checked += (s, e) => App.Config.SCALING = 1.1D;
        HoyoSettingStatic.RadioButtonScale_3x.Checked += (s, e) => App.Config.SCALING = 1.2D;
        HoyoSettingStatic.RadioButtonScale_4x.Checked += (s, e) => App.Config.SCALING = 1.3D;
    }
}