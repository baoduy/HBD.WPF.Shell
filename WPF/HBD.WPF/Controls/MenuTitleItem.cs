#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace HBD.WPF.Controls
{
    public class MenuTitleItem : Control
    {
        //Override default value of Parent Properties
        public new static readonly DependencyProperty FontSizeProperty =
            Control.FontSizeProperty.AddOwner(typeof(MenuTitleItem), new FrameworkPropertyMetadata(16d));

        public new static readonly DependencyProperty FontWeightProperty =
            Control.FontWeightProperty.AddOwner(typeof(MenuTitleItem), new FrameworkPropertyMetadata(FontWeights.Bold));

        public new static readonly DependencyProperty ForegroundProperty =
            Control.ForegroundProperty.AddOwner(typeof(MenuTitleItem), new FrameworkPropertyMetadata(Brushes.LightBlue));

        public new static readonly DependencyProperty BackgroundProperty =
            Control.BackgroundProperty.AddOwner(typeof(MenuTitleItem),
                new FrameworkPropertyMetadata(SystemColors.ControlBrush));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string),
            typeof(MenuTitleItem), new FrameworkPropertyMetadata("Menu Title Item"));

        static MenuTitleItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuTitleItem),
                new FrameworkPropertyMetadata(typeof(MenuTitleItem)));
        }

        public string Header
        {
            get { return GetValue(HeaderProperty) as string; }
            set { SetValue(HeaderProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var c = this.FindParent<ContentPresenter>();
            if (c != null)
            {
                Grid.SetColumn(c, 0);
                Grid.SetRow(c, 0);
                Grid.SetColumnSpan(c, 2);
            }
            var m = c.FindParent<MenuItem>();
            if (m != null)
            {
                m.IsEnabled = false;
                m.HeaderTemplate = null;
            }

            base.OnRender(drawingContext);
        }
    }
}