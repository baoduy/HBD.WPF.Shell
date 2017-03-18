using System.Collections.Generic;
using Prism.Regions;

namespace HBD.WPF.Shell.Services
{
    internal static class Extensions
    {
        public static NavigationParameters ToParameters(this IDictionary<string, object> @this)
        {
            var nav = new NavigationParameters();
            if (@this == null) return nav;

            foreach (var k in @this)
                nav.Add(k.Key, k.Value);
            return nav;
        }
    }
}
