#region

using System;
using HBD.Mef.Shell.Navigation;

#endregion

namespace HBD.WPF.Shell.Navigation
{
    /// <summary>
    ///     The available region name in WPF.Shell.Common.RegionNames If Region Name is Empty Shell will
    ///     execute with default Region: TabRegion
    /// </summary>
    public class RegionNavigationParameter : ViewInfo
    {
        //Default Region is TabRegion.
        public RegionNavigationParameter(Type viewType)
            : this(null, null, viewType)
        {
        }

        public RegionNavigationParameter(string regionName, string viewName)
            : this(regionName, viewName, null)
        {
        }

        public RegionNavigationParameter(string regionName, Type viewType)
            : this(regionName, null, viewType)
        {
        }

        public RegionNavigationParameter(string regionName, string viewName, Type viewType)
            : base(viewName, viewType)
        {
            RegionName = regionName;
        }

        public string RegionName { get; set; }
    }
}