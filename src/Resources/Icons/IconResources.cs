namespace HoyoLauncher.Resources.Icons;

public sealed class IconResources
{
    public static Draw.Icon Icon_16
    {
        get => new(Application.GetResourceStream(new("pack://application:,,,/Resources/Icons/16.ico")).Stream);
    }

    public static Draw.Icon Icon_64
    {
        get => new(Application.GetResourceStream(new("pack://application:,,,/Resources/Icons/64.ico")).Stream);
    }

    public static Draw.Icon Icon_128
    {
        get => new(Application.GetResourceStream(new("pack://application:,,,/Resources/Icons/128.ico")).Stream);
    }

    public static Draw.Icon Icon_256
    {
        get => new(Application.GetResourceStream(new("pack://application:,,,/Resources/Icons/256.ico")).Stream);
    }
}