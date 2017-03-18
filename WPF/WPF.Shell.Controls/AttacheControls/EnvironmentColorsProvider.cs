#region

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HBD.Framework;
using HBD.WPF.Shell.Configuration;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace HBD.WPF.Shell.Controls.AttacheControls
{
    public class EnvironmentColorsProvider : DependencyObject
    {
        public static readonly DependencyProperty EnvironmentBackgroundProperty =
            DependencyProperty.RegisterAttached("EnvironmentBackground", typeof(bool), typeof(EnvironmentColorsProvider),
                new PropertyMetadata(false, EnvironmentBackgroundChanged));

        public static readonly DependencyProperty EnvironmentForegroundProperty =
            DependencyProperty.RegisterAttached("EnvironmentForeground", typeof(bool), typeof(EnvironmentColorsProvider),
                new PropertyMetadata(false, EnvironmentForegroundChanged));

        public static void SetEnvironmentBackground(DependencyObject element, bool value)
            => element.SetValue(EnvironmentBackgroundProperty, value);

        public static bool GetEnvironmentBackground(DependencyObject element)
            => (bool) element.GetValue(EnvironmentBackgroundProperty);

        public static void SetEnvironmentForeground(DependencyObject element, bool value)
            => element.SetValue(EnvironmentForegroundProperty, value);

        public static bool GetEnvironmentForeground(DependencyObject element)
            => (bool) element.GetValue(EnvironmentForegroundProperty);

        private static void EnvironmentBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;

            var brush = GetBrush();

            d.SetValue(Control.BackgroundProperty, brush);
        }

        private static void EnvironmentForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;

            var brush = GetBrush();

            d.SetValue(Control.ForegroundProperty, brush);
        }

        private static Brush GetBrush()
        {
            var shell = ServiceLocator.Current?.TryResolve<IWpfConfigManager>();
            if (shell == null) return new SolidColorBrush(Colors.LightGray);

            if (shell.ShellConfig.Environment.StartsWithIgnoreCase("Dev"))
                return Application.Current.TryFindResource(ResourceKeys.DevBackgroundBrush) as Brush;

            if (shell.ShellConfig.Environment.StartsWithIgnoreCase("Sit")
                || shell.ShellConfig.Environment.StartsWithIgnoreCase("Staging"))
                return Application.Current.TryFindResource(ResourceKeys.StagingBackgroundBrush) as Brush;

            if (shell.ShellConfig.Environment.StartsWithIgnoreCase("Uat")
                || shell.ShellConfig.Environment.StartsWithIgnoreCase("PrePrd"))
                return Application.Current.TryFindResource(ResourceKeys.PrePrdBackgroundBrush) as Brush;

            if (shell.ShellConfig.Environment.StartsWithIgnoreCase("Prd"))
                return Application.Current.TryFindResource(ResourceKeys.PrdBackgroundBrush) as Brush;

            return new SolidColorBrush(Colors.LightGray);
        }
    }
}