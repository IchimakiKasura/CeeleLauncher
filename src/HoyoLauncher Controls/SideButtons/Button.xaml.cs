namespace HoyoLauncher.HoyoLauncher_Controls.SideButtons;

public partial class Button : UserControl
{
    public RoutedEventHandler Click;
    readonly static DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(string), typeof(Button));
    readonly static DependencyProperty BGProperty =
        DependencyProperty.Register("BG", typeof(Brush), typeof(Button), new(new BrushConverter().ConvertFromString("#f4cb99") as Brush));
    readonly static DependencyProperty ButtonToolTipProperty =
        DependencyProperty.Register("ButtonToolTip", typeof(string), typeof(Button), new("Tooltip Temp"));
    readonly static DependencyProperty CanvasMarginProperty =
        DependencyProperty.Register("CanvasMargin", typeof(Thickness), typeof(Button), new(new Thickness(0,0,0,0)));

    public string Image
    {
        get => (string)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
    public Brush BG
    {
        get => (Brush)GetValue(BGProperty);
        set => SetValue(BGProperty, value);
    }
    public string ButtonToolTip
    {
        get => (string)GetValue(ButtonToolTipProperty);
        set => SetValue(ButtonToolTipProperty, value);
    }
    public Thickness CanvasMargin
    {
        get => (Thickness)GetValue(CanvasMarginProperty);
        set => SetValue(CanvasMarginProperty, value);
    }

    public Button()
    {
        InitializeComponent();

        Loaded += delegate
        {
            Border ToolTipSideButton_Border = (Border)mainButton.Template.FindName("ToolTipSideButton_Border", mainButton);
            var NewWidth = ToolTipSideButton_Border.ActualWidth - 20;
            Canvas.SetLeft(ToolTipSideButton_Border, -(NewWidth += -Canvas.GetLeft(ToolTipSideButton_Border)));
        };
    }

    protected virtual void OnClicked(RoutedEventArgs Event) =>
        Click?.Invoke(this, Event);

    private void Clicked(object s, RoutedEventArgs Event) =>
        OnClicked(Event);
}