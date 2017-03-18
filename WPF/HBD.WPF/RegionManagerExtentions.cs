#region

using System.Linq;
using System.Windows;
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using HBD.WPF.Shell.Core;
using Prism.Regions;
using HBD.Framework;
using System.Collections.Generic;

#endregion

namespace HBD.WPF
{
    public static class RegionManagerExtentions
    {
        internal static IEnumerable<IRegion> Filter([NotNull]this IRegionCollection @this, params string[] regionNames)
        {
            if (regionNames.Length <= 0) return @this;
            return @this.Where(r => regionNames.Any(f => f.EqualsIgnoreCase( r.Name)));
        }
        
        public static RegionInfoItem FindRegionInfoItem([NotNull]this IRegionManager @this, [NotNull]object viewOrViewModel,params string[] regionNames)
        {
            Guard.ArgumentIsNotNull(@this, nameof(IRegionManager));
            Guard.ArgumentIsNotNull(viewOrViewModel, nameof(viewOrViewModel));

            var regionInfo = new RegionInfoItem();

            var item = (from r in @this.Regions.Filter(regionNames)
                        from v in r.Views
                        let view = v as FrameworkElement
                        let d = view?.DataContext
                        where d != null && d == viewOrViewModel || v == viewOrViewModel
                        select new { RegionName = r.Name, View = view }).FirstOrDefault();

            if (item == null) return regionInfo;

            regionInfo.RegionName = item.RegionName;
            regionInfo.View = item.View;

            return regionInfo;
        }

        public static RegionInfoItem FindRegionInfoItem([NotNull]this IRegionManager @this, [NotNull]string viewName, params string[] regionNames)
        {
            Guard.ArgumentIsNotNull(@this, nameof(IRegionManager));
            Guard.ArgumentIsNotNull(viewName, nameof(viewName));

            var regionInfo = new RegionInfoItem();

            var item = (from r in @this.Regions.Filter(regionNames)
                        let v = r.GetView(viewName)as FrameworkElement
                        where v != null
                        select new { RegionName = r.Name, View = v }).FirstOrDefault();

            if (item == null) return regionInfo;

            regionInfo.RegionName = item.RegionName;
            regionInfo.View = item.View;

            return regionInfo;
        }
    }
}