#region

using System.Collections.Generic;
using System.Windows;
using HBD.Mef.Shell;
using HBD.Mef.Shell.Navigation;
using HBD.Mef.Shell.Services;
using HBD.WPF;
using HBD.WPF.Shell.Modularity;
using HBD.WPF.Shell.Navigation;
using Prism.Mef.Modularity;
using Prism.Modularity;

#endregion

namespace WPF.Demo.Module
{
    [ModuleExport(typeof(DemoStartupModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class DemoStartupModule : WpfModuleBase
    {
        protected override void MenuConfiguration(IShellMenuService menuSet)
        {
            menuSet.Menu("_Demo1")
                .Children
                .AddTitle("Title 1")
                .AndNavigation("Demo View 1")
                .WithToolTip("Hello Tooltip")
                .ForRegion(null, typeof(View1))
                .AndNavigation("Demo View 2")
                .ForRegion(typeof(ColorViewer))
                .AndSeparator()
                .AddTitle("Title 2")
                .AndNavigation("Demo View 11")
                .WithToolTip("Hello Tooltip")
                .ForRegion(typeof(View1))
                .AndNavigation("Demo View 22")
                .For<ColorViewer>();

            menuSet.Menu("_Demo2");
        }

        public override void Initialize()
        {
            base.Initialize();
            Application.Current.Resources.MergedDictionaries.Add(this.GetResourceDictionary());
        }

        protected override IEnumerable<IViewInfo> GetStartUpViewTypes()
        {
            yield return new ViewInfo(typeof(View1));
        }
    }
}