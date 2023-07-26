﻿namespace HoyoLauncher;

public partial class ShortTour : Window
{
    [StaticWindow]
    public static ShortTour ShortTourWindow { get; set; }

    static int CurrentImage;
    public ShortTour()
    {
        InitializeComponent();

        ShortTourWindow = this;

        CurrentImage = 1;
        
        HoyoWindow.BLACK_THING.Visibility = Visibility.Visible;
        WindowDrag.MouseDown += App.DragMove<ShortTour>;

        MainImageSource.Background = new ImageBrush(ImageLocation());

        NextButton.Click += (s, e) =>
        {
            switch(++CurrentImage)
            {
                case 8: NextButton.Content = "Close"; break;
                case 9: Close();                      return;
            }

            MainImageSource.Background = new ImageBrush(ImageLocation());

            if (CurrentImage < 1) return;
            
            BackButton.Foreground = App.ConvertColorFromString("#dba867");
            BackButton.IsEnabled = true;

        };

        BackButton.Click += (s, e) =>
        {
            --CurrentImage;

            MainImageSource.Background = new ImageBrush(ImageLocation());
            NextButton.Content = "Next";

            if (CurrentImage is not 1) return;

            BackButton.Foreground = Brushes.Black;
            BackButton.IsEnabled = false;
        };
    }

    private static BitmapImage ImageLocation()
        => new(new Uri($"pack://application:,,,/Resources/ShortTour/{CurrentImage}.png"));

    protected override void OnClosed(EventArgs e)
    {
        HoyoWindow.BLACK_THING.Visibility = Visibility.Collapsed;
        base.OnClosed(e);
    }
}