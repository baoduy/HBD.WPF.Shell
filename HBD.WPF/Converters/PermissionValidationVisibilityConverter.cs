#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HBD.Mef.Shell.Core;
using HBD.Mef.Shell.Navigation;
using Microsoft.Practices.ServiceLocation;

// ReSharper disable TryCastAlwaysSucceeds
// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

#endregion

namespace HBD.WPF.Converters
{
    public class PermissionValidationVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || ServiceLocator.Current == null) return Visibility.Visible;

            IPermissionValidationInfo info = null;

            if (value is IPermissionValidationInfo)
                info = value as IPermissionValidationInfo;

            if (value is IMenuInfo)
                info = ((IMenuInfo) value).PermissionValidation;

            if (info == null) return Visibility.Visible;

            //var service = InternalShare.AuthenticationService;

            //if (service != null && service.IsValid(info))
            //    return Visibility.Visible;

                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}