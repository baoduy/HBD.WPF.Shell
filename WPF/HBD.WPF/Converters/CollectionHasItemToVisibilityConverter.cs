#region

using System;
using System.Globalization;
using System.Windows;

#endregion

namespace HBD.WPF.Converters
{
    public class CollectionHasItemToVisibilityConverter : IsCollectionHasItemConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool) base.Convert(value, targetType, parameter, culture) ? Visibility.Visible : Visibility.Hidden;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (Visibility?) value == Visibility.Visible;
    }
}