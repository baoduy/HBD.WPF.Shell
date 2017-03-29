#region

using System;
using System.Windows.Input;
using HBD.Framework.Core;

#endregion

namespace HBD.WPF.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, bool> _canExecuteAction;
        private readonly Action<object> _executeAction;

        public RelayCommand(Action<object> executeAction) : this(executeAction, p => true)
        {
        }

        public RelayCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
        {
            Guard.ArgumentIsNotNull(executeAction, nameof(executeAction));
            Guard.ArgumentIsNotNull(canExecuteAction, nameof(canExecuteAction));

            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter) => _canExecuteAction.Invoke(parameter);

        public virtual void Execute(object parameter) => _executeAction?.Invoke(parameter);
    }
}