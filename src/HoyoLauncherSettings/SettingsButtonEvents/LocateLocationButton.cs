namespace HoyoLauncher.HoyoLauncherSettings;

public partial class HoyoSettings
{
    void LocationButtonClick(object s, RoutedEventArgs e)
    {
        using var Folder = new Forms.FolderBrowserDialog();
        e.Handled = true;

        if (Folder.ShowDialog() is Forms.DialogResult.Cancel) return;

        string path = Folder.SelectedPath;

        switch (((Button)e.Source).Name)
        {
            case "GI_LOCATE": App.Config.GI_DIR = GI_DIR_TXT.Text = path; break;
            case "HSR_LOCATE": App.Config.HSR_DIR = HSR_DIR_TXT.Text = path; break;
            case "HI3_LOCATE": App.Config.HI3_DIR = HI3_DIR_TXT.Text = path; break;
        }
    }

    void LocationImageButtonClick(object s, RoutedEventArgs e)
    {
        var ImageFile = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "PNG|*.png|JPG|*.jpg;*.jpeg|GIF|*.gif|BMP|*.bmp|All Files|*.*"
        };
        e.Handled = true;

        if (ImageFile.ShowDialog() is false) return;
        
        BG_DIR_TXT.Text = App.Config.CUSTOM_BACKGROUND = ImageFile.FileName;
    }
}   