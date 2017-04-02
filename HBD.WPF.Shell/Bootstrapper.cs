#region

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using HBD.Framework;
using HBD.Mef.Logging;
using HBD.WPF.Shell.Configuration;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Logging;
using HBD.WPF.Shell.Regions;
using HBD.WPF.Shell.Services;
using Microsoft.Practices.ServiceLocation;
using Prism;
using Prism.Logging;
using Prism.Mef;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell
{
    internal class Bootstrapper : MefBootstrapper
    {
        private IWpfConfigManager ShellConfigManager { get; } = new WpfConfigManager();

        protected override ILoggerFacade CreateLogger() => new WpfLogger();

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));

            //Load Shell Extension dlls and styles
            ShellConfigManager.ImportShellBinaries(AggregateCatalog);

            //Load Modules
            ShellConfigManager.ImportModuleBinaries(AggregateCatalog);
        }

        protected override void RegisterBootstrapperProvidedTypes()
        {
            base.RegisterBootstrapperProvidedTypes();

            Container.ComposeExportedValue<IRegionNavigationContentLoader>(
                new ScopedRegionNavigationContentLoader(Container.GetExportedValue<IServiceLocator>()));

            Container.ComposeExportedValue(Logger as ILogger);
            Container.ComposeExportedValue(ShellConfigManager);
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(TabControl), new TabControlRegionAdapter(
                Container.GetExportedValue<IRegionBehaviorFactory>()));

            return mappings;
        }

        protected override DependencyObject CreateShell() => Container.GetExportedValue<WorkspaceWindow>();

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Container.ComposeParts(Application.Current);

            var manager = Container.GetExportedValue<IWpfConfigManager>();
            var option = Container.GetExportedValue<IShellOptionService>();
            option.LoadSetting();

            //Set default theme if option.Theme is NULL.
            if (option.Theme.IsNullOrEmpty())
                option.Theme = manager.ShellConfig.DefaultTheme;

            //Load Shell Icon
            if (manager.ShellConfig.Logo.IsNotNullOrEmpty())
            {
                var fullPath = Path.GetFullPath(manager.ShellConfig.Logo);
                if (File.Exists(fullPath)
                    && !Application.Current.Resources.Contains(ResourceKeys.MainWindowIcon))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(fullPath);
                    bmp.EndInit();
                    Application.Current.Resources.Add(ResourceKeys.MainWindowIcon, bmp);
                }
            }

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Title = manager.ShellConfig.Title;
            Application.Current.MainWindow.Closed += MainWindow_Closed;
            Application.Current.MainWindow.ContentRendered += MainWindow_ContentRendered;
            Application.Current.MainWindow.Show();
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            var shellViewModel = Application.Current.MainWindow.CastAs<IActiveAware>();
            if (shellViewModel == null || shellViewModel.IsActive) return;
            shellViewModel.IsActive = true;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var option = Container.GetExportedValue<IShellOptionService>();
            option.SaveSetting();

            (Logger as IDisposable)?.Dispose();
            Application.Current.Shutdown();
        }
    }
}