namespace HoyoLauncher.Core.EventHandlers.WindowEvents;

[Events]
public sealed class Window__
{
    public static void Method() =>
        HoyoWindow.HoyoTitleIMG.Visibility = App.Config.CHECKBOX_TITLE ? Visibility.Visible : Visibility.Collapsed;
}