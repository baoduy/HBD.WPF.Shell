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
            Guard.ArgumentIsNotNull(parameter, nameof(parameter));

            if (parameter is NavigationCommandParameter)
                this.ExecuteCommand((NavigationCommandParameter)parameter);
            else this.ExecuteRegion((NavigationRegionParameter)parameter);
        }

        protected virtual void ExecuteCommand([NotNull]NavigationCommandParameter command)
        {
            if (command.Command.CanExecute(command.CommandParameter))
                command.Command.Execute(command.CommandParameter);
        }

        protected virtual void ExecuteRegion([NotNull]NavigationRegionParameter command)
           => NavigationService.RequestNavigate(command);
    }
}