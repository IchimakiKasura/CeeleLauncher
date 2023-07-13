namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings
{
    void LocationButtonClick(object s, RoutedEventArgs e)
    {
        using var Folder = new Forms.FolderBrowserDialog();
        string path = null;
        e.Handled = true;

        if (Folder.ShowDialog() is Forms.DialogResult.Cancel)
            return;

        path = Folder.SelectedPath;

        switch(((Button)e.Source).Name)
        {
            case "GI_LOCATE": App.Config.GI_DIR = GI_DIR_TXT.Text = path; break;
            case "HSR_LOCATE": App.Config.HSR_DIR = HSR_DIR_TXT.Text = path; break;
            case "HI3_LOCATE": App.Config.HI3_DIR = HI3_DIR_TXT.Text = path; break;
        }
    }
}