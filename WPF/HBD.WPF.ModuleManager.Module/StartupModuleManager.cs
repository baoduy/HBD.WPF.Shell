#region

using HBD.Mef.Shell;
using HBD.Mef.Shell.Services;
using HBD.WPF.ModuleManager.Module.Views;
using HBD.WPF.Shell.Modularity;
using HBD.WPF.Shell.Navigation;
using Prism.Mef.Modularity;
using Prism.Modularity;

#endregion

namespace HBD.WPF.ModuleManager.Module
{
    [ModuleExport(typeof(StartupModuleManager), InitializationMode = InitializationMode.WhenAvailable)]
    public class StartupModuleManager : ModuleBase
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
                .For(new NavigationRegionParameter(typeof(MainView)));
        }
    }
}