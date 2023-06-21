namespace HoyoLauncher;

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

        MainImageSource.Source = ImageLocation();

        NextButton.Click += (s, e) =>
        {
            switch(++CurrentImage)
            {
                case 5: NextButton.Content = "Close"; break;
                case 6: Close();                      return;
            }

            MainImageSource.Source = ImageLocation();

            if(CurrentImage < 1) return;
            
            BackButton.Foreground = App.ConvertColorFromString("#dba867");
            BackButton.IsEnabled = true;

        };

        BackButton.Click += (s, e) =>
        {
            --CurrentImage;

            MainImageSource.Source = ImageLocation();
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
