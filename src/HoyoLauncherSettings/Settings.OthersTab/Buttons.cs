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
        App.Config.Reset();

        HoyoSettingStatic.GI_DIR_TXT.Text = App.Config.GI_DIR;
        HoyoSettingStatic.HSR_DIR_TXT.Text = App.Config.HSR_DIR;
        HoyoSettingStatic.HI3_DIR_TXT.Text = App.Config.HI3_DIR;
        HoyoSettingStatic.BG_DIR_TXT.Text = App.Config.CUSTOM_BACKGROUND;

        HoyoSettingStatic.RadioButtonToTray.IsChecked = true;
        HoyoSettingStatic.RadioButtonBackground.IsChecked = App.Config.CHECKBOX_BACKGROUND;
        HoyoSettingStatic.RadioButtonSelectiveStartup.IsChecked = App.Config.CHECKBOX_LAST_GAME;
        HoyoSettingStatic.RadioButtonDisableTitle.IsChecked = App.Config.CHECKBOX_TITLE;

        HoyoMessageBox.Show(HoyoWindow.Title, "Settings has been Reset!", HoyoSettingStatic);
    }
    
    static void ResetLocationButtonClick(object s, RoutedEventArgs e)
    {
        HoyoSettingStatic.GI_DIR_TXT.Text = "";
        HoyoSettingStatic.HSR_DIR_TXT.Text = "";
        HoyoSettingStatic.HI3_DIR_TXT.Text = "";

        HoyoMessageBox.Show(HoyoWindow.Title, "Locations are now cleared!", HoyoSettingStatic);
    }
}