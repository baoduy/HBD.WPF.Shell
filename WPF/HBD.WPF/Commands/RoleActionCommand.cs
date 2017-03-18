#region

using System;
using HBD.Mef.Shell.Core;

#endregion

namespace HBD.WPF.Commands
{
    public class RoleActionCommand : RelayCommand
    {
        public RoleActionCommand(Action<object> executeAction, IPermissionValidationInfo info) : base(executeAction)
        {
            PermissionInfo = info;
        }

        public RoleActionCommand(Action<object> executeAction, Func<object, bool> canExecuteAction,
            IPermissionValidationInfo info) : base(executeAction, canExecuteAction)
        {
            PermissionInfo = info;
        }

        public IPermissionValidationInfo PermissionInfo { get; }

        public override bool CanExecute(object parameter)
        {
            if (PermissionInfo == null) return base.CanExecute(parameter);

            //var service = null; InternalShare.AuthenticationService;

            //if (service != null && !service.IsValid(PermissionInfo))
            //    return false;

            return base.CanExecute(parameter);
        }
    }
}