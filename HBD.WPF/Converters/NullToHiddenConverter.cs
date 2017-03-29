#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HBD.Framework;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Convert Null to Hidden and vise versa.
    /// </summary>
    public class NullToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value.IsNull() ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility && (Visibility) value == Visibility.Visible ? true : false;
    }
}