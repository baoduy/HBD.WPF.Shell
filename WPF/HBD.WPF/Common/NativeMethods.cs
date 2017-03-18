using System;
using System.Runtime.InteropServices;

namespace HBD.WPF.Common
{
    internal static class NativeMethods
    {
        [DllImport("User32.dll", EntryPoint = "GetWindowLong")]
        internal static extern int GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
