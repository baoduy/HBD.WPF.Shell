#region

using HBD.Mef.Shell;
using HBD.Mef.Shell.Services;
using HBD.WPF.Shell.Modularity;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.ShellOptionManager.Module.Views;
using Prism.Mef.Modularity;
using Prism.Modularity;

#endregion

namespace HBD.WPF.ShellOptionManager.Module
{
    [ModuleExport(typeof(StartupShellOption), InitializationMode = InitializationMode.WhenAvailable)]
    public class StartupShellOption : WpfModuleBase
    {
        protected override void MenuConfiguration(IShellMenuService menuSet)
        {
            menuSet.Menu("_Workspace")
                .Children
                .Navigation("Options")
                .WithToolTip("This feature allow you to manage the Option of this application.")
                .WithIcon(GetResource("OptionIcon"))
                .DisplayAt(1)
                //.AndValidForOperations(DefaultOperations.UpdateSetting.ToString())
                .For(new RegionNavigationParameter(typeof(ShellOptionView)));
        }
    }
}