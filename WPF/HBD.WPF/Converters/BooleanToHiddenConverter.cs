#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Boolean.True to Visibility.Collapsed. Boolean.False to Visibility.Visible.
    /// </summary>
    public sealed class BooleanToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bValue = false;
            if (value is bool)
                bValue = (bool) value;
            return bValue ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value as Visibility? == Visibility.Collapsed;
    }
}