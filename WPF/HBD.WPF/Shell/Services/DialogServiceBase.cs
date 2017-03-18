#region

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Management.Instrumentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using HBD.Framework;
using HBD.Framework.Core;
using HBD.WPF.Controls;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.ViewModels;
using HBD.WPF.Windows;
using Microsoft.Win32;
using Prism;

#endregion

namespace HBD.WPF.Shell.Services
{
    public abstract class DialogServiceBase : IDialogService
    {
        static DialogServiceBase()
        {
            OpenedWindowCache = new ConcurrentDictionary<object, Window>();
            OpenedChildWindowCache = new ConcurrentDictionary<object, ModelWindow>();
        }

        protected virtual IDialogWindow CreateWindow(object parentViewModel, FrameworkElement contentView,
            DialogType type)
        {
            IDialogWindow win = null;
            switch (type)
            {
                case DialogType.Dialog:
                    win = CreateDialogWindow(parentViewModel, contentView);
                    break;

                case DialogType.Model:
                    win = CreateChildWindow(parentViewModel, contentView);
                    break;

                case DialogType.Auto:
                    throw new NotSupportedException(type.ToString());
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            win.Owner = GetLatestOpeningWindow();
            AddToCache(parentViewModel, win);

            return win;
        }

        protected virtual IDialogOption CreateDialogOption(string title, FrameworkElement view, object parameters,
            DialogButtons buttons = DialogButtons.OkCancel)
        {
            var option = new DialogOption {Title = title, View = view, Parameters = parameters, Buttons = buttons};

            var viewTitle = view.CastAs<IViewTitle>();
            if (viewTitle != null && option.Title.IsNullOrEmpty())
                option.Title = viewTitle.ViewTitle ?? viewTitle.ViewHeader;

            return option;
        }

        protected virtual IDialogOption CreateDialogOption<TView>(string title, object dataContext, object parameters,
            DialogButtons buttons = DialogButtons.OkCancel)
        {
            var view = GetView<TView>();
            if (dataContext != null)
                view.DataContext = dataContext;
            return CreateDialogOption(title, view, parameters, buttons);
        }

        #region Abstracts

        protected abstract DialogWindow CreateDialogWindow(object parentViewModel, FrameworkElement contentView);

        protected abstract ModelWindow CreateChildWindow(object parentViewModel, FrameworkElement contentView);

        protected abstract FrameworkElement GetView<TView>(object dataContext = null);

        #endregion

        #region Window Cache

        private void AddToCache(object parentViewModel, IDialogWindow window)
        {
            var w = window as Window;
            if (w != null)
                AddWindowToCache(parentViewModel, w);
            else AddModelWindowToCache(parentViewModel, (ModelWindow) window);
        }

        private void RemoveFromCache(object parentViewModel, IDialogWindow window)
        {
            if (window is Window)
                RemoveWindowFromCache(parentViewModel);
            else RemoveModelWindowFromCache(parentViewModel);
        }

        protected static ConcurrentDictionary<object, Window> OpenedWindowCache { get; }

        //protected virtual bool HasWindowDialog(object parentViewModel)
        //{
        //    if (!OpenedWindowCache.ContainsKey(parentViewModel)) return false;
        //    Window win = null;
        //    if (OpenedWindowCache.TryGetValue(parentViewModel, out win))
        //        return true;

        //    OpenedWindowCache.TryRemove(parentViewModel, out win);
        //    return false;
        //}

        protected virtual void AddWindowToCache(object parentViewModel, Window window)
            => OpenedWindowCache.TryAdd(parentViewModel, window);

        protected virtual void RemoveWindowFromCache(object parentViewModel)
        {
            Window win = null;
            OpenedWindowCache.TryRemove(parentViewModel, out win);
        }

        protected virtual Window GetLatestOpeningWindow()
        {
            if (OpenedWindowCache.Count <= 0) return Application.Current.MainWindow;

            Window win = null;
            var key = OpenedWindowCache.Keys.LastOrDefault();

            if (key != null)
                OpenedWindowCache.TryGetValue(key, out win);

            return win ?? Application.Current.MainWindow;
        }

        #endregion Window Cache

        #region ModelWindowCache

        protected static ConcurrentDictionary<object, ModelWindow> OpenedChildWindowCache { get; }

        protected virtual bool HasModelWindowDialog(object parentViewModel)
        {
            if (!OpenedChildWindowCache.ContainsKey(parentViewModel)) return false;

            ModelWindow win = null;

            if (OpenedChildWindowCache.TryGetValue(parentViewModel, out win))
                return true;

            OpenedChildWindowCache.TryRemove(parentViewModel, out win);
            return false;
        }

        protected virtual void AddModelWindowToCache(object parentViewModel, ModelWindow window)
        {
            FrameworkElement parentView;

            if (parentViewModel is IShellViewModel)
                parentView = Application.Current.MainWindow;
            else
                parentView = GetLatestOpeningWindow()
                    .FindChild<UserControl>()
                    .FirstOrDefault(c => c.DataContext == parentViewModel);

            if (parentView == null)
                throw new InstanceNotFoundException($"The view of {parentViewModel.GetType().FullName}");

            //Find IAddChild control and add itself in.
            var p = parentView.FindParent<Grid>();
            if (p == null)
                throw new InstanceNotFoundException("IAddChild control");

            Grid.SetRow(window, 0);
            Grid.SetColumn(window, 0);

            if (p.RowDefinitions.Count > 0)
                Grid.SetRowSpan(window, p.RowDefinitions.Count);
            if (p.ColumnDefinitions.Count > 0)
                Grid.SetColumnSpan(window, p.ColumnDefinitions.Count);

            ((IAddChild) p).AddChild(window);

            OpenedChildWindowCache.TryAdd(parentViewModel, window);
        }

        protected virtual void RemoveModelWindowFromCache(object parentViewModel)
        {
            if (!OpenedChildWindowCache.ContainsKey(parentViewModel)) return;
            ModelWindow window;
            OpenedChildWindowCache.TryRemove(parentViewModel, out window);

            var p = window?.FindParent<Grid>();
            p?.Children.Remove(window);
        }

        #endregion ChildWindowCache

        #region ShowDialog

        protected IDialogWindow GetOrCreateDialog(object parentViewModel, FrameworkElement view, DialogType type)
        {
            switch (type)
            {
                case DialogType.Auto:
                    return CreateWindow(parentViewModel, view,
                        HasModelWindowDialog(parentViewModel) ? DialogType.Dialog : DialogType.Model);

                case DialogType.Model:
                {
                    if (HasModelWindowDialog(parentViewModel))
                        throw new Exception("Model is showing.");
                    return CreateWindow(parentViewModel, view, DialogType.Model);
                }
                case DialogType.Dialog:
                default:
                    return CreateWindow(parentViewModel, view, DialogType.Dialog);
            }
        }

        //Main Method
        /// <summary>
        /// </summary>
        /// <param name="parentViewModel"></param>
        /// <param name="dialogOption"></param>
        /// <param name="dialogCallback"></param>
        public void ShowDialog(object parentViewModel, IDialogOption dialogOption,
            Action<IDialogResult> dialogCallback = null)
        {
            Guard.ArgumentIsNotNull(parentViewModel, nameof(parentViewModel));
            if (parentViewModel.GetType().IsStringOrValueType())
                throw new AggregateException("parentViewModel object cant be an string or value type");

            var busyIndicator = parentViewModel as IBusyIndicator;

            try
            {
                if (busyIndicator != null && !busyIndicator.IsBusy)
                    busyIndicator.ShowBusy();

                var window = GetOrCreateDialog(parentViewModel, dialogOption.View, dialogOption.DialogType);
                var dialogAware = dialogOption.View.CastAs<IDialogAware>();
                var activeAware = dialogOption.View.CastAs<IActiveAware>();

                window.Title = dialogOption.Title;
                window.DialogCommands.Clear();

                window.Shown += (s, e) =>
                {
                    dialogAware?.DialogActivating(dialogOption.Parameters, dialogOption.CustomCommands);
                    if (activeAware != null && !activeAware.IsActive)
                        activeAware.IsActive = true;

                    window.DialogCommands.AddRange(dialogOption.CustomCommands);
                };

                window.Closing += (s, e) =>
                {
                    var closingResult = new DialogClosingResult(window.MessageBoxResult);
                    dialogAware?.DialogClosing(closingResult);
                    e.Cancel = closingResult.Cancel;
                };
                window.Closed += (s, e) =>
                {
                    var closedResult = new DialogResult(window.MessageBoxResult, dialogOption.Parameters);
                    dialogAware?.DialogClosed(closedResult);
                    dialogCallback?.Invoke(closedResult);

                    RemoveFromCache(parentViewModel, (IDialogWindow) s);
                };

                window.ShowDialog();
            }
            finally
            {
                busyIndicator.HideBusy();
            }
        }

        public void ShowDialog(object parentViewModel, Action<IDialogOption> dialogOption,
            Action<IDialogResult> dialogCallback = null)
        {
            var option = new DialogOption();
            dialogOption.Invoke(option);
            ShowDialog(parentViewModel, option, dialogCallback);
        }

        public void ShowDialog<TView>(object parentViewModel, Action<IDialogOption> dialogOption,
            Action<IDialogResult> dialogCallback = null)
        {
            var option = CreateDialogOption<TView>(null, null, null);
            dialogOption.Invoke(option);
            ShowDialog(parentViewModel, option, dialogCallback);
        }

        public void ShowDialog(object parentViewModel, string title, FrameworkElement view, object parameters,
                DialogButtons buttons = DialogButtons.OkCancel,
                Action<IDialogResult> dialogCallback = null)
            => ShowDialog(parentViewModel, CreateDialogOption(title, view, parameters, buttons), dialogCallback);

        public void ShowDialog<TView>(object parentViewModel, string title, object dataContext, object parameters,
                DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null)
            =>
            ShowDialog(parentViewModel, CreateDialogOption<TView>(title, dataContext, parameters, buttons),
                dialogCallback);

        public void ShowDialog(object parentViewModel, string title, FrameworkElement view,
                DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null)
            => ShowDialog(parentViewModel, CreateDialogOption(title, view, buttons), dialogCallback);

        public void ShowDialog<TView>(object parentViewModel, string title, object dataContext,
                DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null)
            => ShowDialog(parentViewModel, CreateDialogOption<TView>(title, dataContext, buttons), dialogCallback);

        public void ShowDialog<TView>(object parentViewModel, string title = null,
                DialogButtons buttons = DialogButtons.OkCancel, Action<IDialogResult> dialogCallback = null)
            => ShowDialog(parentViewModel, CreateDialogOption<TView>(title, null, null, buttons), dialogCallback);

        #endregion

        #region Open and Save Dialog

        private static FileDialog CreateNewFileDialog(bool isSaveDialog)
        {
            FileDialog file = null;
            if (isSaveDialog)
                file = new SaveFileDialog {Title = "Save File Dialog"};
            else
                file = new OpenFileDialog {Title = "Open File Dialog"};

            file.CheckPathExists = true;
            file.CheckFileExists = true;
            file.AddExtension = true;

            return file;
        }

        private static IDialogResult ShowFileDialog(bool isSaveDialog, Action<FileDialog> options)
        {
            var dialog = CreateNewFileDialog(false);
            options?.Invoke(dialog);

            return dialog.ShowDialog() != true
                ? new DialogResult(MessageBoxResult.Cancel, null)
                : new DialogResult(MessageBoxResult.OK, isSaveDialog ? dialog.FileName : (object) dialog.FileNames);
        }

        private static IDialogResult ShowFolderDialog(Action<FolderDialog> options)
        {
            var dialog = new FolderDialog {Title = "Select Folder"};
            options?.Invoke(dialog);

            return dialog.ShowDialog() != true
                ? new DialogResult(MessageBoxResult.Cancel, null)
                : new DialogResult(MessageBoxResult.OK, dialog.SelectedPath);
        }

        public virtual IDialogResult FileOpenDialog(string defaultExt, string filter) => FileOpenDialog(p =>
        {
            p.DefaultExt = defaultExt;
            p.Filter = filter;
        });

        public virtual IDialogResult FileOpenDialog(Action<FileDialog> options) => ShowFileDialog(false, options);

        public virtual IDialogResult FileSaveDialog(string defaultExt, string filter) => FileSaveDialog(p =>
        {
            p.DefaultExt = defaultExt;
            p.Filter = filter;
        });

        public virtual IDialogResult FileSaveDialog(Action<FileDialog> options) => ShowFileDialog(true, options);

        public IDialogResult FolderOpenDialog(string defaultPath = "Desktop")
            => FolderOpenDialog(f => f.SelectedPath = defaultPath);

        public IDialogResult FolderOpenDialog(Action<FolderDialog> options) => ShowFolderDialog(options);

        #endregion Open and Save Dialog

        #region BackgroundWorker

        //public void RunWorker(object parentViewModel, string title, Action<BackgroundWorker> doWorkAction)
        //{
        //    this.ShowDialogAsync(parentViewModel, op =>
        //    {
        //        op.
        //    });
        //}

        #endregion
    }
}