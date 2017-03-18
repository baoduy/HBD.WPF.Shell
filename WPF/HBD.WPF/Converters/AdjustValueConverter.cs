#region

using System;
using System.Globalization;
using System.Windows.Data;
using HBD.Framework;

#endregion

namespace HBD.WPF.Converters
{
    public class AdjustValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.ChangeType<double>();
            var adj = parameter.ChangeType<double>();

            return val + adj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.ChangeType<double>();
            var adj = parameter.ChangeType<double>();

            return val - adj;
        }
    }
}