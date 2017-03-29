#region

using System;
using System.Windows;
using HBD.WPF.Controls;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;
using Microsoft.Win32;

#endregion

namespace HBD.WPF.Shell.Services
{
    public interface IDialogService
    {
        void ShowDialog(object parentViewModel, IDialogOption dialogOption, Action<IDialogResult> dialogCallback = null);

        void ShowDialog(object parentViewModel, Action<IDialogOption> dialogOption,
            Action<IDialogResult> dialogCallback = null);

        void ShowDialog<TView>(object parentViewModel, Action<IDialogOption> dialogOption,
            Action<IDialogResult> dialogCallback = null);

        void ShowDialog(object parentViewModel, string title, FrameworkElement view, object parameters,
            DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null);

        void ShowDialog<TView>(object parentViewModel, string title, object dataContext, object parameters,
            DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null);

        void ShowDialog(object parentViewModel, string title, FrameworkElement view,
            DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null);

        void ShowDialog<TView>(object parentViewModel, string title, object dataContext,
            DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null);

        void ShowDialog<TView>(object parentViewModel, string title = null,
            DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null);

        #region FileDialog

        IDialogResult FileOpenDialog(string defaultExt, string filter);

        IDialogResult FileOpenDialog(Action<FileDialog> options);

        IDialogResult FileSaveDialog(string defaultExt, string filter);

        IDialogResult FileSaveDialog(Action<FileDialog> options);

        IDialogResult FolderOpenDialog(string defaultPath = "Desktop");

        IDialogResult FolderOpenDialog(Action<FolderDialog> options);

        #endregion FileDialog

        #region Background Worker

        //void RunWorker(object parentViewModel, string title, Action<BackgroundWorker> doWorkAction);

        #endregion
    }
}