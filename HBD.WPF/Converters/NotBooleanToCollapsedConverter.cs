#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Boolean.True to Visibility.Visible. Boolean.False to Visibility.Collapsed.
    /// </summary>
    public sealed class NotBooleanToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bValue = false;
            if (value is bool)
                bValue = (bool) value;
            return bValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value as Visibility? == Visibility.Visible;
    }
}