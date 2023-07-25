namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings
{
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Settings_ScrollBar.Height = 270;

        GI_DIR_TXT.Text = App.Config.GI_DIR;
        HSR_DIR_TXT.Text = App.Config.HSR_DIR;
        HI3_DIR_TXT.Text = App.Config.HI3_DIR;
        BG_DIR_TXT.Text = App.Config.CUSTOM_BACKGROUND;
        
        RadioButtonBackground.IsChecked = App.Config.CHECKBOX_BACKGROUND;
        RadioButtonSelectiveStartup.IsChecked = App.Config.CHECKBOX_LAST_GAME;
        RadioButtonDisableTitle.IsChecked = App.Config.CHECKBOX_TITLE;

        switch (App.Config.EXIT_MODE)
        {
            default:
                HoyoMessageBox.Show("ERROR", "ExitMode value is invalid,\ronly accepts \"1\" and \"2\"\rDefaulting to 1", HoyoSettingStatic);
                RadioButtonToTray.IsChecked = true;
                App.Config.EXIT_MODE = 1;
                App.Config.SaveConfig();
                break;
            case 1: RadioButtonToTray.IsChecked = true; break;
            case 2: RadioButtonToExit.IsChecked = true; break;
        }
        
        switch (App.Config.SCALING)
        {
            default:
                HoyoMessageBox.Show("ERROR", "WindowScale value is invalid,\ronly accepts \"1.0\" to \"1.3\"\rDefaulting to 1.0", HoyoSettingStatic);
                RadioButtonScale_1x.IsChecked = true;
                App.Config.SCALING = 1.0D;
                App.Config.SaveConfig();
                break;
            case 1.0D: RadioButtonScale_1x.IsChecked = true; break;
            case 1.1D: RadioButtonScale_2x.IsChecked = true; break;
            case 1.2D: RadioButtonScale_3x.IsChecked = true; break;
            case 1.3D: RadioButtonScale_4x.IsChecked = true; break;
        }

        BG_DIR_TXT.TextChanged += (s, e) => BG_DIR_TXT.Text = App.Config.CUSTOM_BACKGROUND;
    }
}