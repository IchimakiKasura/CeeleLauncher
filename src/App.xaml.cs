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

        protected override void OnStartup(StartupEventArgs e)
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            _mutex = new(true, appName, out bool createdNew);

            if (!createdNew)
                if (MessageBox.Show("Only one instance at a time!", "Warning",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning) is MessageBoxResult.OK)
                    Environment.Exit(0);

            base.OnStartup(e);
        }

    }
}
