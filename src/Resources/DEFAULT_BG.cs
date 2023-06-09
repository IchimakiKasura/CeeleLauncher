namespace HoyoLauncher.Resources;

public sealed class DefaultBG
{
    public static ImageBrush GENSHIN_BG
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_GENSHIN_BG.jpg", UriKind.RelativeOrAbsolute)));
    }

    public static ImageBrush HSR_BG
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_HSR_BG.jpg", UriKind.RelativeOrAbsolute)));
    }

    public static ImageBrush HI3_BG
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_HI3_BG.jpg", UriKind.RelativeOrAbsolute)));
    }

    public static ImageBrush ZZZ_BG
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_ZZZ_BG.jpg", UriKind.RelativeOrAbsolute)));
    }
}