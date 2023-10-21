namespace HoyoLauncher.HoyoLauncherSettings.Settings.OthersTab;

sealed class HoyoSettingsButtons
{
    public static void SetButtons()
    {
        HoyoSettingStatic.Button_ResetSettings.Click += ResetSettingsButtonClick;
        HoyoSettingStatic.Button_ClearLocationTexts.Click += ResetLocationButtonClick;
        HoyoSettingStatic.Button_ClearNoLocation.Click += ResetNonDIRLocationButtonClick;
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

    static async void ResetNonDIRLocationButtonClick(object s, RoutedEventArgs e)
    {
        List<(TextBox config, HoyoGames AbsoluteName)> GameConfigs = new()
        {
            (HoyoSettingStatic.GI_DIR_TXT, HoyoGames.GenshinImpact),
            (HoyoSettingStatic.HSR_DIR_TXT, HoyoGames.HonkaiStarRail),
            (HoyoSettingStatic.HI3_DIR_TXT, HoyoGames.HonkaiImpactThird)
        };

        foreach(var (config, name) in GameConfigs)
        {
            HoyoMain.ValidateSettings(await GameConfigRead.GetConfig(config.Text), name, out bool IsInvalidGame);

            if(IsInvalidGame) {
                config.Text = "";
            }
        }

        HoyoMessageBox.Show(HoyoWindow.Title, "Locations are now cleared!", HoyoSettingStatic);
    }
}