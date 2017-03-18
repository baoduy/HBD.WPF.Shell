#region

using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Converters
{
    public class IsCollectionHasItemConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as IList;
            return collection?.Count > 0;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}