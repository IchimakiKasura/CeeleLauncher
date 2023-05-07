﻿using System.Threading;

namespace HoyoLauncherProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Mutex _mutex;
        public static Forms.NotifyIcon nIcon = new();
        public static readonly string TempBG = Path.Combine(Path.GetTempPath(), "HoyoverseBG", "bg.mp4");

        protected override void OnStartup(StartupEventArgs e)
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            _mutex = new(true, appName, out bool createdNew);

            if (!createdNew)
                if (MessageBox.Show("Only one instance at a time!", "Warning",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning) is MessageBoxResult.OK)
                    Environment.Exit(0);

            var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            menuDropAlignmentField.SetValue(null, !SystemParameters.MenuDropAlignment || menuDropAlignmentField is null);

            // Tray Icon
            nIcon.Icon = new Draw.Icon(GetResourceStream(new Uri("pack://application:,,,/Images/icons/16.ico")).Stream);
            nIcon.Visible = false;
            nIcon.Text = appName;
            nIcon.Click += (s, e) =>
            {
                if (MainWindow.WindowState is not WindowState.Minimized) return;

                MainWindow.Show();
                MainWindow.WindowState = WindowState.Normal;
                MainWindow.ShowInTaskbar = true;
                nIcon.Visible = false;
            };

            nIcon.BalloonTipText = "HoyoLauncher will be running in the background.";
            nIcon.BalloonTipTitle = appName;
            nIcon.BalloonTipIcon = Forms.ToolTipIcon.Info;

            // Tray Context Menu
            nIcon.ContextMenuStrip = new();

            // Extracting the BG to play
            if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "HoyoverseBG")))
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "HoyoverseBG"));

            if(!File.Exists(TempBG))
                File.WriteAllBytes(TempBG, AppResources.Resources.bg);
            
            base.OnStartup(e);
        }

    }
}
