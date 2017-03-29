#region

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    /// <summary>
    ///     Convert  MenuItemRole.TopLevelHeader or  MenuItemRole.TopLevelItem to Boolean True.
    /// </summary>
    public class IsTopMenuLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MenuItemRole))
                return false;

            var r = (MenuItemRole) value;
            return r == MenuItemRole.TopLevelHeader || r == MenuItemRole.TopLevelItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return value;

            return (bool) value ? MenuItemRole.TopLevelItem : MenuItemRole.SubmenuItem;
        }
    }
}