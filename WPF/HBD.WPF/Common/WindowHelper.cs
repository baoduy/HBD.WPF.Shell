#region

using System.Windows;
using System.Windows.Interop;

#endregion

namespace HBD.WPF.Common
{
    internal static class WindowHelper
    {
        private const int GwlStyle = -16;
        private const int WsMaximizebox = 0x00010000;
        private const int WsMinimizebox = 0x00020000;

        /// <summary>
        ///     Disables the maximize functionality of a WPF window.
        /// </summary>
        /// The WPF window to be modified.
        public static void DisableMaximize(Window window)
        {
            lock (window)
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                var windowStyle = NativeMethods.GetWindowLongPtr(hWnd, GwlStyle);
                NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle & ~WsMaximizebox);
            }
        }

        /// <summary>
        ///     Disables the minimize functionality of a WPF window.
        /// </summary>
        /// The WPF window to be modified.
        public static void DisableMinimize(Window window)
        {
            lock (window)
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                var windowStyle = NativeMethods.GetWindowLongPtr(hWnd, GwlStyle);
                NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle & ~WsMinimizebox);
            }
        }

        /// <summary>
        ///     Enables the maximize functionality of a WPF window.
        /// </summary>
        /// The WPF window to be modified.
        public static void EnableMaximize(Window window)
        {
            lock (window)
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                var windowStyle = NativeMethods.GetWindowLongPtr(hWnd, GwlStyle);
                NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle | WsMaximizebox);
            }
        }

        /// <summary>
        ///     Enables the minimize functionality of a WPF window.
        /// </summary>
        /// The WPF window to be modified.
        public static void EnableMinimize(Window window)
        {
            lock (window)
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                var windowStyle = NativeMethods.GetWindowLongPtr(hWnd, GwlStyle);
                NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle | WsMinimizebox);
            }
        }

        /// <summary>
        ///     Toggles the enabled state of a WPF window's maximize functionality.
        /// </summary>
        /// The WPF window to be modified.
        public static void ToggleMaximize(Window window)
        {
            lock (window)
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                var windowStyle = NativeMethods.GetWindowLongPtr(hWnd, GwlStyle);

                if ((windowStyle | WsMaximizebox) == windowStyle)
                    NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle & ~WsMaximizebox);
                else
                    NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle | WsMaximizebox);
            }
        }

        /// <summary>
        ///     Toggles the enabled state of a WPF window's minimize functionality.
        /// </summary>
        /// The WPF window to be modified.
        public static void ToggleMinimize(Window window)
        {
            lock (window)
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                var windowStyle = NativeMethods.GetWindowLongPtr(hWnd, GwlStyle);

                if ((windowStyle | WsMinimizebox) == windowStyle)
                    NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle & ~WsMinimizebox);
                else
                    NativeMethods.SetWindowLongPtr(hWnd, GwlStyle, windowStyle | WsMinimizebox);
            }
        }
    }
}