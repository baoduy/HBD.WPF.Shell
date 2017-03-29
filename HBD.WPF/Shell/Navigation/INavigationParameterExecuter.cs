#region

using System;
using System.Collections.Generic;
using HBD.Mef.Shell.Navigation;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.Navigation
{
    public interface INavigationParameterExecuter
    {
        void Execute(INavigationInfo menuInfo, Action<NavigationResult> regionNavigationCallback);

        void Execute(IEnumerable<INavigationParameter> navigations, Action<NavigationResult> regionNavigationCallback);

        void Execute(INavigationParameter parameter, Action<NavigationResult> regionNavigationCallback);
    }
}