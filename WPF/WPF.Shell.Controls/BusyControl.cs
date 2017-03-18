#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace HBD.WPF.Shell.Controls
{
    public class BusyControl : Control
    {
        public static readonly DependencyProperty BusyMessageProperty = DependencyProperty.Register("BusyMessage",
            typeof(string), typeof(BusyControl));

        //Override default value of Parent Properties
        public new static readonly DependencyProperty ForegroundProperty =
            Control.ForegroundProperty.AddOwner(typeof(BusyControl), new FrameworkPropertyMetadata(Brushes.White));

        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register(
            "TextForeground", typeof(Brush), typeof(BusyControl), new FrameworkPropertyMetadata(Brushes.Blue));

        public new static readonly DependencyProperty BackgroundProperty =
            Control.BackgroundProperty.AddOwner(typeof(BusyControl),
                new FrameworkPropertyMetadata(new SolidColorBrush {Color = Colors.Gray, Opacity = 0.7}));

        static BusyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyControl),
                new FrameworkPropertyMetadata(typeof(BusyControl)));
        }

        public BusyControl()
        {
            base.Visibility = Visibility.Collapsed;
        }

        public string BusyMessage
        {
            get { return GetValue(BusyMessageProperty) as string; }
            set { SetValue(BusyMessageProperty, value); }
        }

        public string TextForeground
        {
            get { return GetValue(TextForegroundProperty) as string; }
            set { SetValue(TextForegroundProperty, value); }
        }

        public new Visibility Visibility => base.Visibility;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property != IsEnabledProperty) return;
            base.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}