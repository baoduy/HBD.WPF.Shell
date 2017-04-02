#region

using System.Windows;
using HBD.WPF.Common;

#endregion

namespace HBD.WPF.AttacheControls
{
    public class WindowCustomizer
    {

        #region CanMaximize

        public static readonly DependencyProperty CanMaximize = DependencyProperty.RegisterAttached("CanMaximize",
            typeof(bool), typeof(Window), new PropertyMetadata(true, OnCanMaximizeChanged));

        private static void OnCanMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null) return;

            RoutedEventHandler loadedHandler = null;
            loadedHandler = delegate
            {
                if ((bool) e.NewValue)
                    WindowHelper.EnableMaximize(window);
                else
                    WindowHelper.DisableMaximize(window);
                window.Loaded -= loadedHandler;
            };

            if (!window.IsLoaded)
                window.Loaded += loadedHandler;
            else
                loadedHandler(null, null);
        }

        public static void SetCanMaximize(DependencyObject d, bool value) => d.SetValue(CanMaximize, value);

        public static bool GetCanMaximize(DependencyObject d) => (bool) d.GetValue(CanMaximize);

        #endregion CanMaximize

        #region CanMinimize

        public static readonly DependencyProperty CanMinimize = DependencyProperty.RegisterAttached("CanMinimize",
            typeof(bool), typeof(Window), new PropertyMetadata(true, OnCanMinimizeChanged));

        private static void OnCanMinimizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null) return;

            RoutedEventHandler loadedHandler = null;
            loadedHandler = delegate
            {
                if ((bool) e.NewValue)
                    WindowHelper.EnableMinimize(window);
                else
                    WindowHelper.DisableMinimize(window);
                window.Loaded -= loadedHandler;
            };

            if (!window.IsLoaded)
                window.Loaded += loadedHandler;
            else
                loadedHandler(null, null);
        }

        public static void SetCanMinimize(DependencyObject d, bool value) => d.SetValue(CanMinimize, value);

        public static bool GetCanMinimize(DependencyObject d) => (bool) d.GetValue(CanMinimize);

        #endregion CanMinimize
    }
}