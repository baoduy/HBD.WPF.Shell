#region

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HBD.WPF.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color)
                return new SolidColorBrush((Color) value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            return brush?.Color ?? value;
        }
    }
}