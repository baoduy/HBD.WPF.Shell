using System;
using System.Globalization;
using System.Windows.Data;
using HBD.Framework.Core;

namespace HBD.WPF.Converters
{
    /// <summary>
    /// Check the type of object is inherited from parameter Type.
    /// </summary>
    public class IsTypeOfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            Guard.ArgumentIsNotNull(parameter, nameof(parameter));

            var valueType = value.GetType();
            // ReSharper disable once PossibleNullReferenceException
            var checkType = parameter as Type ?? parameter.GetType();

            return checkType.IsAssignableFrom(valueType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
