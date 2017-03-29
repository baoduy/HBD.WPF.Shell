#region

using System;
using System.ComponentModel.Composition;
using HBD.WPF.Shell.Core;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace HBD.WPF.Shell.Services
{
    public abstract class MessageBoxServiceBase : IMessageBoxService
    {
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IServiceLocator Container { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IDialogService DialogService { protected get; set; }

        protected abstract IMessageBoxOption CreateMessageBoxOption(string message, string title,
            MessageIconType iconType);

        #region MessageBox

        public virtual void Alert(object parentViewModel, string message, string title = "Alert",
                Action<IDialogResult> dialogCallback = null)
            =>
            DialogService.ShowDialog(parentViewModel, CreateMessageBoxOption(message, title, MessageIconType.Alert),
                dialogCallback);

        public virtual IDialogResult ConfirmDialog(object parentViewModel, string message, string title = "Confirmation")
        {
            IDialogResult result = null;
            var option = CreateMessageBoxOption(message, title, MessageIconType.Confirm);
            option.DialogType = DialogType.Dialog;
            DialogService.ShowDialog(parentViewModel, option, rs => result = rs);
            return result;
        }

        public virtual void Confirm(object parentViewModel, string message, string title = "Confirmation",
                Action<IDialogResult> dialogCallback = null)
            =>
            DialogService.ShowDialog(parentViewModel, CreateMessageBoxOption(message, title, MessageIconType.Confirm),
                dialogCallback);

        public virtual void Info(object parentViewModel, string message, string title = "Information",
                Action<IDialogResult> dialogCallback = null)
            =>
            DialogService.ShowDialog(parentViewModel, CreateMessageBoxOption(message, title, MessageIconType.Info),
                dialogCallback);

        #endregion MessageBox
    }
}