namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;

sealed class HoyoSettingsButtons
{
    public static void SetButtons()
    {
        HoyoSettingStatic.Button_ResetSettings.Click += ResetSettingsButtonClick;
        HoyoSettingStatic.Button_ClearLocationTexts.Click += ResetLocationButtonClick;
    }

    static void ResetSettingsButtonClick(object s, RoutedEventArgs e)
    {
        AppSettings.Settings.Default.Reset();

        HoyoSettingStatic.GI_DIR_TXT.Text = AppSettings.Settings.Default.GENSHIN_IMPACT_DIR;
        HoyoSettingStatic.HSR_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_STAR_RAIL_DIR;
        HoyoSettingStatic.HI3_DIR_TXT.Text = AppSettings.Settings.Default.HONKAI_IMPACT_THIRD_DIR;

        HoyoSettingStatic.RadioButtonTray.IsChecked = AppSettings.Settings.Default.CHECKBOX_MINIMIZE_TRAY;
        HoyoSettingStatic.RadioButtonBackground.IsChecked = AppSettings.Settings.Default.CHECKBOX_BACKGROUND;
        HoyoSettingStatic.RadioButtonSelectiveStartup.IsChecked = AppSettings.Settings.Default.CHECKBOX_LASTGAME;
        HoyoSettingStatic.RadioButtonDisableTitle.IsChecked = AppSettings.Settings.Default.CHECKBOX_TITLE;

        MessageBox.Show("Settings has been Reset!", HoyoWindow.Title);
    }

    static void ResetLocationButtonClick(object s, RoutedEventArgs e)
    {
        HoyoSettingStatic.GI_DIR_TXT.Text = "";
        HoyoSettingStatic.HSR_DIR_TXT.Text = "";
        HoyoSettingStatic.HI3_DIR_TXT.Text = "";

        MessageBox.Show("Locations are now cleared!", HoyoWindow.Title);
    }
}