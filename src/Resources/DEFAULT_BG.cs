namespace HoyoLauncher.Resources;

// yeah yeah yeah, i should've put it in Resources.resx but having that "default" annoys me
public sealed class DefaultBG
{
    public static ImageBrush DEFAULT =>
        new(new BitmapImage(new("pack://application:,,,/Resources/defaultBG.jpg", UriKind.RelativeOrAbsolute)));
    public static ImageBrush GENSHIN_BG =>
        new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_GENSHIN_BG.jpg", UriKind.RelativeOrAbsolute)));
    public static ImageBrush HSR_BG =>
        new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_HSR_BG.jpg", UriKind.RelativeOrAbsolute)));
    public static ImageBrush HI3_BG =>
        new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_HI3_BG.jpg", UriKind.RelativeOrAbsolute)));
    public static ImageBrush ZZZ_BG =>
        new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_ZZZ_BG.jpg", UriKind.RelativeOrAbsolute)));
    public static ImageBrush TOT_BG =>
        new(new BitmapImage(new("pack://application:,,,/Resources/DEFAULT_TOT_BG.jpg", UriKind.RelativeOrAbsolute)));
}