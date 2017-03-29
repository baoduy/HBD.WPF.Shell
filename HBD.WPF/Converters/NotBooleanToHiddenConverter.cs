#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Boolean.True to Visibility.Visible. Boolean.False to Visibility.Hidden.
    /// </summary>
    public sealed class NotBooleanToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bValue = false;
            if (value is bool)
                bValue = (bool) value;
            return bValue ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value as Visibility? == Visibility.Visible;
    }
}