using static HoyoLauncher.HoyoLauncherSettings.HoyoSettings;

namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;
sealed class HoyoRadioButtons
{
    // kinda messed up but it'll work, just need some better coding; ref wont work on the AppSettings.Settings.Default
    public static void SetRadioButtons()
    {
        _hoyosettings.RadioButtonTray.IsChecked = AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY;
        _hoyosettings.RadioButtonBackground.IsChecked = AppSettings.Settings.Default.CHECKBOX_BACKGROUND;
        _hoyosettings.RadioButtonSelectiveStartup.IsChecked = AppSettings.Settings.Default.CHECKBOX_LASTGAME;

        _hoyosettings.RadioButtonTray_Click.MouseDown += (s, e) =>
            SetSettingsValue(e, () =>
            {
                AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY = (bool)(_hoyosettings.RadioButtonTray.IsChecked = !_hoyosettings.RadioButtonTray.IsChecked);
            });
        _hoyosettings.RadioButtonBackground_Click.MouseDown += (s, e) =>
            SetSettingsValue(e, () =>
            {
                AppSettings.Settings.Default.CHECKBOX_BACKGROUND = (bool)(_hoyosettings.RadioButtonBackground.IsChecked = !_hoyosettings.RadioButtonBackground.IsChecked);
            });
        _hoyosettings.RadioButtonSelectiveStartup_Click.MouseDown += (s, e) =>
            SetSettingsValue(e, () =>
            {
                AppSettings.Settings.Default.CHECKBOX_LASTGAME = (bool)(_hoyosettings.RadioButtonSelectiveStartup.IsChecked = !_hoyosettings.RadioButtonSelectiveStartup.IsChecked);
            });
    }

    static void SetSettingsValue(MouseButtonEventArgs e, Action action)
    {
        if (e.ChangedButton is MouseButton.Left)
            action();
    }
}