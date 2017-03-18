#region

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using HBD.Framework.Core;

#endregion

namespace HBD.WPF.Converters
{
    internal class IsEnabledForegroundConverter : IValueConverter
    {
        private readonly Brush _disabledBrush = Brushes.LightGray;
        private readonly Brush _enabledBrush = Brushes.Black;

        public IsEnabledForegroundConverter()
        {
        }

        public IsEnabledForegroundConverter(Brush enabledBrush, Brush disabledBrush)
        {
            Guard.ArgumentIsNotNull(enabledBrush, nameof(enabledBrush));
            Guard.ArgumentIsNotNull(disabledBrush, nameof(disabledBrush));

            _enabledBrush = enabledBrush;
            _disabledBrush = disabledBrush;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (bool) value == false)
                return _disabledBrush;
            return _enabledBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value == _enabledBrush;
    }
}