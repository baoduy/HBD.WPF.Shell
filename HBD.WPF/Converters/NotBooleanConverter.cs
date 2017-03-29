#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Convert True to False and vice versa.
    /// </summary>
    public class NotBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool) value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool) value;
    }
}