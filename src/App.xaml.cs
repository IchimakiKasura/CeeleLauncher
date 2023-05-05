global using Forms = System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace HoyoLauncherProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Mutex _mutex;
        public static Forms.NotifyIcon nIcon = new();

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
            nIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Images/HoyoVerseIcon.ico")).Stream);
            nIcon.Visible = false;
            nIcon.Text = appName;
            nIcon.Click += (s, e) =>
            {
                if (MainWindow.WindowState is WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                    MainWindow.ShowInTaskbar = true;
                    nIcon.Visible = false;
                }
            };
            nIcon.BalloonTipText = "HoyoLauncher will be running in the background.";
            nIcon.BalloonTipTitle = appName;
            nIcon.BalloonTipIcon = Forms.ToolTipIcon.None;

            base.OnStartup(e);
        }

    }
}
