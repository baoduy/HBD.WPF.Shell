#region

using HBD.Mef.Shell;
using HBD.Mef.Shell.Services;
using HBD.WPF.ModuleManager.Module.Views;
using HBD.WPF.Shell.Modularity;
using HBD.WPF.Shell.Navigation;
using Prism.Mef.Modularity;

#endregion

namespace HBD.WPF.ModuleManager.Module
{
    [ModuleExport(typeof(StartupModuleManager))]
    public class StartupModuleManager : WpfModuleBase
    {
        protected override void MenuConfiguration(IShellMenuService menuSet)
        {
            menuSet.Menu("_Workspace")
                .Children
                    .AddNavigation("Module Management")
                    .WithToolTip("This feature allow you to manage the Modules, that are using on this application.")
                    .WithIcon(GetResource("MM_MenuIcon"))
                    .DisplayAt(0)
                    //.AndValidForRoles(DefaultRoles.Administrator)
                    .For(new RegionNavigationParameter(typeof(MainView)));
        }
    }
}