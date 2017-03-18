#region

using System;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Services
{
    public interface IMessageBoxService
    {
        #region MessageBox

        void Info(object parentViewModel, string message, string title = "Information",
            Action<IDialogResult> dialogCallback = null);

        void Alert(object parentViewModel, string message, string title = "Alert",
            Action<IDialogResult> dialogCallback = null);

        void Confirm(object parentViewModel, string message, string title = "Confirmation",
            Action<IDialogResult> dialogCallback = null);

        IDialogResult ConfirmDialog(object parentViewModel, string message, string title = "Confirmation");

        #endregion MessageBox
    }
}