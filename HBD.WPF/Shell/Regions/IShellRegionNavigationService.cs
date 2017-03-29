#region

using System;
using System.Collections.Generic;
using HBD.Framework.Attributes;
using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.Navigation;

#endregion

namespace HBD.WPF.Shell.Regions
{
    public interface IShellRegionNavigationService
    {
        /// <summary>
        /// Load IShellStatusView, IShellMenuView, IShellTitleView, IShellMainView
        /// These Views allow to load one time only.
        /// </summary>
        void LoadShellViews();

        void SetViewTitle(string title);

        void LoadView([NotNull]IViewInfo viewInfo);

        void RequestNavigate([NotNull]RegionNavigationParameter command);
        void RequestNavigate([NotNull]string regionName, IDictionary<string, object> parameters = null);
        void RequestNavigate([NotNull]Type viewType, IDictionary<string, object> parameters = null);
        void RequestNavigate<TViewType>(IDictionary<string, object> parameters = null);
        void Close([NotNull]object viewOrViewModel);
    }
}