namespace HoyoLauncher;

public partial class FirstRunWindow : Window
{
    static int CurrentImage;
    public FirstRunWindow()
    {
        InitializeComponent();

        CurrentImage = 1;
        
        HoyoWindow.BLACK_THING.Opacity = 0.5;
        WindowDrag.MouseDown += (s, e) => { if (e.ChangedButton is MouseButton.Left) DragMove(); };

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
            
            BackButton.Foreground = new BrushConverter().ConvertFromString("#dba867") as Brush;
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
        HoyoWindow.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }
}
