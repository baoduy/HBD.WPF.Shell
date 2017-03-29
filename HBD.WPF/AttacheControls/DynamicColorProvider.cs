#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using HBD.WPF.Core;

#endregion

namespace HBD.WPF.AttacheControls
{
    public class DynamicColorProvider : DependencyObject
    {
        public static readonly DependencyProperty DynamicBackgroundProperty =
            DependencyProperty.RegisterAttached("DynamicBackground", typeof(bool), typeof(DynamicColorProvider),
                new PropertyMetadata(false, DynamicBackgroundChanged));

        private static readonly RandomColorProvider RandomColorProvider = new RandomColorProvider();

        public static void SetDynamicBackground([NotNull]DependencyObject element, bool value)
        {
            Guard.ArgumentIsNotNull(element,nameof(element));
            element.SetValue(DynamicBackgroundProperty, value);
        }

        public static bool GetDynamicBackground([NotNull] DependencyObject element)
        {
            Guard.ArgumentIsNotNull(element, nameof(element));
            return (bool) element.GetValue(DynamicBackgroundProperty);
        }

        private static void DynamicBackgroundChanged([NotNull]DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(Control.BackgroundProperty, new SolidColorBrush(RandomColorProvider.NextSequence()));
    }
}