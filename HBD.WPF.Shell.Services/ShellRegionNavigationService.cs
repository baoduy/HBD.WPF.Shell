#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using HBD.Framework;
using HBD.Framework.Attributes;
using HBD.Framework.Exceptions;
using HBD.Mef.Shell.Navigation;
using HBD.Mef.Shell.Views;
using HBD.WPF.Shell.Infos;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.Regions;
using HBD.WPF.Shell.ViewModels;
using HBD.WPF.Shell.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(IShellRegionNavigationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellRegionNavigationService : IShellRegionNavigationService
    {
        protected virtual string DefaultRegionName => RegionNames.TabRegion;

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IRegionManager RegionManager { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IServiceLocator ServiceLocator { protected get; set; }

        public virtual void SetViewTitle(string title)
            => RegionManager.RequestNavigate(RegionNames.TitleRegion, typeof(IShellTitleView).FullName, new NavigationParameters { { NavigaParameters.Title, title } });

        public virtual void LoadView([NotNull]IViewInfo viewInfo)
            => RequestNavigate(new RegionNavigationParameter(DefaultRegionName, viewInfo.ViewName, viewInfo.ViewType));

        public virtual void RequestNavigate([NotNull]RegionNavigationParameter command)
        {
            if (command.RegionName.IsNullOrEmpty())
                command.RegionName = DefaultRegionName;

            ValiddateRegion(command.RegionName);

            var view = ServiceLocator.GetInstance(command.ViewType, command.ViewName);

            if (RegionManager.Regions[command.RegionName].Views.Any(v => v == view))
                RegionManager.Regions[command.RegionName].Activate(view);
            else RegionManager.AddToRegion(command.RegionName, view);


            if (!command.RegionName.EqualsIgnoreCase(RegionNames.TabRegion)) return;
            //Set the view title when added view to region target.
            var title = view.CastAs<IViewTitle>();
            if (title != null)
                SetViewTitle(title.ViewHeader);
        }

        public virtual void RequestNavigate([NotNull]string regionName, IDictionary<string, object> parameters = null)
        {
            var view = RegionManager.Regions[regionName].ActiveViews.First();
            RegionManager.RequestNavigate(regionName, view.GetType().FullName, parameters.ToParameters());
        }

        public virtual void RequestNavigate([NotNull]Type viewType, IDictionary<string, object> parameters = null)
        {
            var info = RegionManager.FindRegionInfoItem(viewType.FullName);
            if (info.IsEmpty())
                info.RegionName = DefaultRegionName;

            RegionManager.RequestNavigate(info.RegionName, viewType.FullName, parameters.ToParameters());
        }

        public void RequestNavigate<TViewType>(IDictionary<string, object> parameters = null)
            => RequestNavigate(typeof(TViewType), parameters);

        private void ValiddateRegion([NotNull]string regionName)
        {
            if (regionName.AnyOfIgnoreCase(RegionNames.StatusRegion, RegionNames.MenuRegion,
                RegionNames.TitleRegion, RegionNames.MainRegion)
                && RegionManager.Regions[regionName].Views.Any())
                throw new InvalidException($"The region {regionName} is not allow to add new or replace the existing View.");
        }

        public void LoadShellViews()
        {
            RegionManager.RequestNavigate(RegionNames.StatusRegion, typeof(IShellStatusView).FullName);
            RegionManager.RequestNavigate(RegionNames.MenuRegion, typeof(IShellMenuView).FullName);
            RegionManager.RequestNavigate(RegionNames.TitleRegion, typeof(IShellTitleView).FullName);
            RegionManager.RequestNavigate(RegionNames.MainRegion, typeof(IShellMainView).FullName);
        }

        public void Close([NotNull]object viewOrViewModel)
        {
            var regionInfo = RegionManager.FindRegionInfoItem(viewOrViewModel, RegionNames.TabRegion, RegionNames.LeftRegion, RegionNames.RightRegion);
            if (regionInfo.IsEmpty()) return;

            var region = RegionManager.Regions[regionInfo.RegionName];
            if (!region.Views.Any()) return;

            if (region is AllActiveRegion)
                region.Remove(regionInfo.View);
            else region.Deactivate(regionInfo.View);
        }

        
    }
}