#region

using System.Windows;
using HBD.Framework;

#endregion

namespace HBD.WPF.Shell.Core
{
    public sealed class RegionInfoItem
    {
        public RegionInfoItem()
        {
        }

        public RegionInfoItem(string regionName, FrameworkElement view)
        {
            RegionName = regionName;
            View = view;
        }

        public string RegionName { get; set; }
        public FrameworkElement View { get; set; }

        public bool IsEmpty() => RegionName.IsNullOrEmpty() && View == null;
    }
}