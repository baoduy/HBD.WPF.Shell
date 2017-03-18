#region

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace HBD.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for TitledContentControl.xaml
    /// </summary>
    public partial class TitledContentControl
    {
        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register("HeaderHeight",
            typeof(double), typeof(TitledContentControl), new PropertyMetadata(25d));

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(TitledContentControl),
                new PropertyMetadata(Brushes.LightBlue));

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(TitledContentControl),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register("HeaderBorderBrush", typeof(Brush), typeof(TitledContentControl),
                new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty HeaderBorderThicknessProperty =
            DependencyProperty.Register("HeaderBorderThickness", typeof(Thickness), typeof(TitledContentControl),
                new PropertyMetadata(new Thickness(0)));

        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand",
            typeof(ICommand), typeof(TitledContentControl));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string),
            typeof(TitledContentControl));

        public TitledContentControl()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        public double HeaderHeight
        {
            get { return (double) GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public Brush HeaderBackground
        {
            get { return GetValue(HeaderBackgroundProperty) as Brush; }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public Brush HeaderForeground
        {
            get { return GetValue(HeaderForegroundProperty) as Brush; }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public Brush HeaderBorderBrush
        {
            get { return GetValue(HeaderBorderBrushProperty) as Brush; }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        public Thickness HeaderBorderThickness
        {
            get { return (Thickness) GetValue(HeaderBorderThicknessProperty); }
            set { SetValue(HeaderBorderThicknessProperty, value); }
        }

        public ICommand CloseCommand
        {
            get { return GetValue(CloseCommandProperty) as ICommand; }
            set { SetValue(CloseCommandProperty, value); }
        }
    }
}