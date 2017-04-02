#region

using System;
using System.Collections.Generic;
using HBD.Framework;
using HBD.Framework.Attributes;
using HBD.Mef.Shell.Navigation;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.Navigation
{
    public abstract class NavigationParameterExecuterBase : INavigationParameterExecuter
    {
        public void Execute(INavigationInfo navigationObject, Action<NavigationResult> regionNavigationCallback)
        {
            if (navigationObject == null) return;
            Execute(navigationObject.NavigationParameters, regionNavigationCallback);
        }

        public void Execute(IEnumerable<INavigationParameter> navigations, Action<NavigationResult> regionNavigationCallback)
            => navigations?.ForEach(n => Execute(n, regionNavigationCallback));

        public abstract void Execute([NotNull]INavigationParameter parameter, Action<NavigationResult> regionNavigationCallback);
    }
}