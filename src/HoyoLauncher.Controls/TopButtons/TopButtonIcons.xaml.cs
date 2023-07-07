namespace HoyoLauncher.Controls.TopButtons;

public partial class HoyoTopButton : UserControl
{
    [Category("Behaviour")]
    public RoutedEventHandler Click;
    readonly static DependencyProperty ImageProperty =
    DependencyProperty.Register("Image", typeof(ImageSource), typeof(HoyoTopButton));
    readonly static DependencyProperty ImageMarginProperty =
    DependencyProperty.Register("ImageMargin", typeof(Thickness), typeof(HoyoTopButton));
    readonly static DependencyProperty ButtonToolTipProperty =
        DependencyProperty.Register("ButtonToolTip", typeof(string), typeof(HoyoTopButton), new("Tooltip Temp"));

    public ImageSource Image
    {
        get => (ImageSource)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
    public Thickness ImageMargin
    {
        get => (Thickness)GetValue(ImageMarginProperty);
        set => SetValue(ImageMarginProperty, value);
    }
    public string ButtonToolTip
    {
        get => (string)GetValue(ButtonToolTipProperty);
        set => SetValue(ButtonToolTipProperty, value);
    }

    public HoyoTopButton()
    {
        InitializeComponent();

        Loaded += delegate
        {
            Border ToolTipSideButton_Border = (Border)MainButton.Template.FindName("ToolTipSideButton_Border", MainButton);

            Canvas.SetLeft(ToolTipSideButton_Border, -((ToolTipSideButton_Border.ActualWidth / 2) - 7));
        };
    }

    public static implicit operator Button(HoyoTopButton source)
    {
        Button btn = source.MainButton;
        btn.Name = source.Name;
        return btn;
    }
    
    protected virtual void OnClicked(RoutedEventArgs Event) =>
        Click?.Invoke(this, Event);
    
    private void Clicked(object s, RoutedEventArgs Event) =>
        OnClicked(Event);
}
