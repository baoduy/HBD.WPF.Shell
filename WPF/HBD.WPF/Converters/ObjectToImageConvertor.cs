#region

using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FontAwesome.WPF;
using HBD.Framework;

#endregion

namespace HBD.WPF.Converters
{
    public class ObjectToImageConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            try
            {
                var im = value as Image;
                if (im != null) return im;

                var bm = value as BitmapImage;
                if (bm != null) return new Image {Source = bm};

                var uri = value as Uri;
                if (uri != null) return new Image {Source = new BitmapImage(uri)};

                var awesomeIcon = value as FontAwesome.WPF.FontAwesome;
                if (awesomeIcon != null)
                {
                    var brush = parameter as Brush ?? awesomeIcon.Foreground ?? Brushes.Black;
                    return new Image
                    {
                        Source = ImageAwesome.CreateImageSource(awesomeIcon.Icon, brush),
                        Width = awesomeIcon.Width,
                        Height = awesomeIcon.Height,
                        MaxWidth = awesomeIcon.MaxWidth,
                        MaxHeight = awesomeIcon.MaxHeight
                    };
                }

                if (value is FontAwesomeIcon)
                {
                    var brush = parameter as Brush ?? Brushes.Black;
                    return new Image {Source = ImageAwesome.CreateImageSource((FontAwesomeIcon) value, brush)};
                }

                var str = value as string;
                if (str == null) return value;

                if (!str.StartsWithIgnoreCase("pack://"))
                    try
                    {
                        str = Path.GetFullPath(str);
                    }
                    catch
                    {
                        // ignored
                    }

                // ReSharper disable once AssignNullToNotNullAttribute
                return new Image {Source = new BitmapImage(new Uri(str))};
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value;
    }
}