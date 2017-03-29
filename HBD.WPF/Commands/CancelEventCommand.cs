#region

using System;
using System.ComponentModel;
using System.Windows.Input;
using HBD.Framework.Core;

#endregion

namespace HBD.WPF.Commands
{
    public class CancelEventCommand : ICommand
    {
        private readonly Action<CancelEventArgs> _executeAction;

        public CancelEventCommand(Action<CancelEventArgs> executeAction)
        {
            Guard.ArgumentIsNotNull(executeAction, nameof(executeAction));
            _executeAction = executeAction;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var args = parameter as CancelEventArgs;
            if (args == null)
                throw new ArgumentException("parameter must be a  System.ComponentModel.CancelEventArgs");
            _executeAction(args);
        }
    }
}