#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using HBD.Mef.Logging;
using HBD.Mef.Shell;
using HBD.Mef.Shell.Navigation;
using HBD.Mef.Shell.Services;
using HBD.WPF.Shell.Infos;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.Regions;
using HBD.WPF.Shell.Services;
using HBD.WPF.Shell.ViewModels;
using HBD.WPF.Shell.Views;

#endregion

namespace HBD.WPF.Shell
{
    [Export(typeof(IShellViewModel))]
    [Export(typeof(IShellStartupService))]
    [Export(typeof(IStartupViewCollection))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WorkspaceViewModel : ViewModelBase, IShellViewModel, IShellStartupService
    {
        private IShellOptionService _shellOptionService;
        private IStartupViewCollection StartupViews { get; } = new StartupViewCollection();

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellRegionNavigationService ShellRegionNavigationService { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellOptionService ShellOptionService
        {
            get { return _shellOptionService; }
            set { SetValue(ref _shellOptionService, value); }
        }

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewHeader = viewTitle = string.Empty;
        }

        protected override void Initialize()
        {
            base.Initialize();

            ShowBusyAndTrace("Loading Regions...");
            InitializeRegions();

            ShowBusyAndTrace("Loading Menus...");
            InitializeWorkspaceMenu();

            ShowBusyAndTrace("Loading Modules...");
            InitializeStartupViews();

            StatusService.SetStatus("The application is ready.");
        }

        private void InitializeWorkspaceMenu()
        {
            var menuModel = Container.GetInstance<IShellMenuViewModel>();

            //var title = AuthenticationService?.IsAuthenticated == true
            //    ? AuthenticationService.UserNameWithoutDomain.ToUpper()
            //    : "_Workspace";

            var title = "_Workspace";

            menuModel.Menu("_Workspace")
                .WithIcon(GetResource(ResourceKeys.WorkspaceIcon))
                .WithTitle(title)
                .DisplayAt(0)
                .Children
                .AddSeparator()
                .AddNavigation("_Exit")
                .WithIcon(GetResource(ResourceKeys.ExitIcon))
                .For(new CommandNavigationParameter(Application.Current.MainWindow.Close));

            menuModel.AddNavigation("_Notification Center")
                .WithIcon(GetResource(ResourceKeys.NotificationBlue))
                .WithToolTip("Notification Center")
                .WithAlignment(MenuAlignment.Right)
                .DisplayIconOnly()
                .For(new RegionNavigationParameter(RegionNames.RightRegion, typeof(IShellNotificationCenterView)));
        }

        private void InitializeRegions()
        {
            try
            {
                this.ShellRegionNavigationService.LoadShellViews();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
        }

        private void InitializeStartupViews()
        {
            foreach (var s in StartupViews)
                ShellRegionNavigationService.LoadView(s);
        }

        #region IShellStartupViewCollection

        public IEnumerator<IViewInfo> GetEnumerator() => StartupViews.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(IViewInfo item) => StartupViews.Add(item);

        public void Clear() => StartupViews.Clear();

        public bool Contains(IViewInfo item) => StartupViews.Contains(item);

        public void CopyTo(IViewInfo[] array, int arrayIndex) => StartupViews.CopyTo(array, arrayIndex);

        public bool Remove(IViewInfo item) => StartupViews.Remove(item);

        public int Count => StartupViews.Count;
        public bool IsReadOnly => StartupViews.IsReadOnly;

        public int IndexOf(IViewInfo item) => StartupViews.IndexOf(item);

        public void Insert(int index, IViewInfo item) => StartupViews.Insert(index, item);

        public void RemoveAt(int index) => StartupViews.RemoveAt(index);

        public IViewInfo this[int index]
        {
            get { return StartupViews[index]; }
            set { StartupViews[index] = value; }
        }

        #endregion IShellStartupViewCollection
    }
}