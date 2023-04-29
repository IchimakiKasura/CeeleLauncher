namespace HoyoLauncherProject.Core;

sealed class EventHandlers
{
    private static MainWindow _m;

    public static void Initialize(MainWindow m)
    {
        _m = m;
        m.ExitButton.Click += Exit;
        m.MinButton.Click += Minimize;
        m.TopBorder.MouseDown += TopBarDrag;
    }

    static void Exit(object sender, RoutedEventArgs e) => _m.Close();
    static void Minimize(object sender, RoutedEventArgs e) => _m.WindowState = WindowState.Minimized;
    static void TopBarDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Left) _m.DragMove();
    }
}