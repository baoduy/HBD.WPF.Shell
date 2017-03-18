#region

using System.Linq;
using System.Windows;
using Xcalibur.NativeMethods.Monitors;
using MonitorHelper = Xcalibur.NativeMethods.Extensions.Monitors.MonitorHelper;

#endregion

namespace HBD.WPF
{
    public static class MonitorExtentions
    {
        private static Rect ToRect(this MONITORINFO @this)
            => new Rect(@this.work.left, @this.work.top, @this.work.right, @this.work.bottom);

        public static Rect GetMainMonitor()
            => MonitorHelper.GetMonitors()
                .First(s => s.MonitorInfo.flags == 1)
                .MonitorInfo.ToRect();
    }
}