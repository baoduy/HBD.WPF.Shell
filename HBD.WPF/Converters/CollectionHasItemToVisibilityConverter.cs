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
        {
            var convert = base.Convert(value, targetType, parameter, culture);
            if (convert == null) return Visibility.Hidden;
            return (bool) convert ? Visibility.Visible : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (Visibility?) value == Visibility.Visible;
    }
}