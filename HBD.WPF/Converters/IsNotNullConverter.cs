#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    public class IsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}