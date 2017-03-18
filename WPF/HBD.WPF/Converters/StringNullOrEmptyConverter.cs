#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    public class StringNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrWhiteSpace(value as string);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);
    }
}