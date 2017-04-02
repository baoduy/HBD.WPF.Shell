#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Opposite the int, float, decimal and double value from negative to positive and vice versa.
    /// </summary>
    public class OpositeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            double d;
            return double.TryParse(value.ToString(), out d) ? System.Convert.ChangeType(d*-1, targetType) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);
    }
}