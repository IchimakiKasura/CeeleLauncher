namespace HoyoLauncher.Resources.Buttons.Selection;

public sealed class SelectionResources
{
    public static ImageBrush GenshinImage
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/Buttons/Selection/GenshinSelectionButton.png")));
    }

    public static ImageBrush HonkaiStarRailImage
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/Buttons/Selection/HSRSelectionButton.png")));
    }

    public static ImageBrush HonkaiImpactImage
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/Buttons/Selection/HI3SelectionButton.png")));
    }

    public static ImageBrush ZZZImage
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/Buttons/Selection/ZZZSelectionButton.png")));
    }

    public static ImageBrush TOTImage
    {
        get => new(new BitmapImage(new("pack://application:,,,/Resources/Buttons/Selection/TOTSelectionButton.png")));
    }
}