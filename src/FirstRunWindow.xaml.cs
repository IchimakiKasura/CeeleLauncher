namespace HoyoLauncher;

public partial class FirstRunWindow : Window
{
    int CurrentImage = 1;

    public FirstRunWindow()
    {
        InitializeComponent();
        HoyoWindow.BLACK_THING.Opacity = 0.5;
        WindowDrag.MouseDown += (s, e) => { if (e.ChangedButton is MouseButton.Left) DragMove(); };

        MainImageSource.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/ShortTour/{CurrentImage}.png"));


        NextButton.Click += (s, e) =>
        {
            CurrentImage+=1;

            if(CurrentImage <= 5)
                MainImageSource.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/ShortTour/{CurrentImage}.png"));

            if(CurrentImage is 5)
                NextButton.Content = "Close";
            
            if (CurrentImage is 6) Close();


            if (CurrentImage > 1)
            {
                BackButton.Foreground = new BrushConverter().ConvertFromString("#dba867") as Brush;
                BackButton.IsEnabled = true;
            }
        };

        BackButton.Click += (s, e) =>
        {
            MainImageSource.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/ShortTour/{CurrentImage-=1}.png"));
            NextButton.Content = "Next";

            if (CurrentImage is 1)
            {
                BackButton.Foreground = Brushes.Black;
                BackButton.IsEnabled = false;
            }
        };
    }


    protected override void OnClosed(EventArgs e)
    {
        HoyoWindow.BLACK_THING.Opacity = 0;
        base.OnClosed(e);
    }
}
