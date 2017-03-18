#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    /// Get Type of object
    /// </summary>
    public class OfTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.GetType();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}