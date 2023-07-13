namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings
{
    void ConfirmClick(object s, RoutedEventArgs e)
    {
        bool ErrorOccured = false;

        List<(TextBox config, HoyoGames AbsoluteName)> GameConfigs = new()
        {
            (GI_DIR_TXT, HoyoGames.GenshinImpact),
            (HSR_DIR_TXT, HoyoGames.HonkaiStarRail),
            (HI3_DIR_TXT, HoyoGames.HonkaiImpactThird)
        };

        foreach(var (config, name) in CollectionsMarshal.AsSpan(GameConfigs))
        {
            HoyoMain.ValidateSettings(config.Text, name, out bool IsInvalidGame);

            if(IsInvalidGame)
            {
                HoyoMessageBox.Show("❌ ERROR ❌", $"The \"{name.GAME_NAME}\" config cannot be found!\n or its an incorrect game.",HoyoSettingStatic);
                ErrorOccured = true;
            }
        }

        App.Config.SaveConfig();
        HoyoGames.RefreshDirectory();

        RefreshCurrentSelectedGame();
        
        if(!ErrorOccured)
            Close();
    }

}