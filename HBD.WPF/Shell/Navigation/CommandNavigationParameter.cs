#region

using System;
using System.Windows.Input;
using HBD.Mef.Shell.Navigation;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.Shell.Navigation
{
    public class CommandNavigationParameter : INavigationParameter
    {
        public CommandNavigationParameter(ICommand command, object parameter = null)
        {
            Command = command;
            CommandParameter = parameter;
        }

        public CommandNavigationParameter(Action action)
        {
            Command = new ActionCommand(action);
        }

        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
    }
}