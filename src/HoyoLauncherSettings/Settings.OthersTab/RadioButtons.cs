using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;

namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoRadioButtons
{
    public static void SetRadioButtons()
    {
        _hoyosettings.RadioButtonTray.IsChecked = AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY;
        _hoyosettings.RadioButtonBackground.IsChecked = AppSettings.Settings.Default.CHECKBOX_BACKGROUND;
        _hoyosettings.RadioButtonSelectiveStartup.IsChecked = AppSettings.Settings.Default.CHECKBOX_LASTGAME;

        _hoyosettings.RadioButtonTray_Click.MouseDown += (s, e) =>
            SetSettingsValue(e, "CHECKBOX_MINIMIZE_TRAY", _hoyosettings.RadioButtonTray.IsChecked = !_hoyosettings.RadioButtonTray.IsChecked);
        _hoyosettings.RadioButtonBackground_Click.MouseDown += (s, e) =>
            SetSettingsValue(e, "CHECKBOX_BACKGROUND", _hoyosettings.RadioButtonBackground.IsChecked = !_hoyosettings.RadioButtonBackground.IsChecked);
        _hoyosettings.RadioButtonSelectiveStartup_Click.MouseDown += (s, e) =>
            SetSettingsValue(e, "CHECKBOX_LASTGAME", _hoyosettings.RadioButtonSelectiveStartup.IsChecked = !_hoyosettings.RadioButtonSelectiveStartup.IsChecked);
    }

    static void SetSettingsValue(MouseButtonEventArgs e, string name, bool? value)
    {
        if (e.ChangedButton is MouseButton.Left)
            AppSettings.Settings.Default[name] = (bool)value;
    }
}