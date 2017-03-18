#region

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace HBD.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for ToolbarItem.xaml
    /// </summary>
    public partial class ToolBarItem : Button
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
            typeof(ToolBarItem), new FrameworkPropertyMetadata("ToolBarItem"));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object),
            typeof(ToolBarItem), new FrameworkPropertyMetadata(null));

        public ToolBarItem()
        {
            InitializeComponent();
        }

        [Bindable(true)]
        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        [Bindable(true)]
        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            //It might set null value to this property so that when the value are being setting from Style won't be effected.
            //Call clear will help to clear the custom value.
            if (e.Property != StyleProperty) return;

            if (Icon == null)
                ClearValue(IconProperty);
            if (string.IsNullOrWhiteSpace(Text))
                ClearValue(TextProperty);
        }
    }
}