#region

using System;
using System.ComponentModel.Composition;
using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.Regions;
using Prism.Regions;
using HBD.Framework.Attributes;
using HBD.Framework.Core;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(INavigationParameterExecuter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavigationExecuter : NavigationParameterExecuterBase
    {
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellRegionNavigationService NavigationService { protected get; set; }

        public override void Execute([NotNull]INavigationParameter parameter, Action<NavigationResult> navigationCallback)
        {
            if (parameter == null) return;

            if (parameter is CommandNavigationParameter)
                this.ExecuteCommand((CommandNavigationParameter)parameter);
            else if (parameter is RegionNavigationParameter)
                this.ExecuteRegion((RegionNavigationParameter)parameter);
            else if (parameter is ActionNavigationParameter)
                this.ExecuteAction((ActionNavigationParameter)parameter);
            else throw new NotSupportedException(parameter.GetType().FullName);
        }

        protected virtual void ExecuteCommand([NotNull]CommandNavigationParameter command)
        {
            if (command.Command.CanExecute(command.CommandParameter))
                command.Command.Execute(command.CommandParameter);
        }

        protected virtual void ExecuteRegion([NotNull]RegionNavigationParameter command)
           => NavigationService.RequestNavigate(command);

        protected virtual void ExecuteAction([NotNull]ActionNavigationParameter action)
            => action.Action.Invoke();
    }
}