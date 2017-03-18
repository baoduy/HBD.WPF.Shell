#region

using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using HBD.WPF.Controls;
using HBD.WPF.EventArguments;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.UI.Controls;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.Shell.UI.ViewModels
{
    [Export(typeof(IShellNotificationCenterViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellNotificationCenterViewModel : ViewModelBase, IShellNotificationCenterViewModel
    {
        //private ICommand _clearAllCommand;

        private ICommand _itemClickedCommand;

        //private ICommand _itemClosedCommand;
        private ICommand _itemLoadedCommand;

        public ShellNotificationCenterViewModel()
        {
            //ItemClosedCommand = new ActionCommand(OnItemClosed);
            ItemClickedCommand = new ActionCommand(OnItemClicked);
            ItemLoadedCommand = new ActionCommand(OnItemLoaded);
            //ClearAllCommand = new ActionCommand(ClearAll);
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public INavigationParameterExecuter NavigationExecuter { get; set; }

        //public ICommand ClearAllCommand
        //{
        //    get { return _clearAllCommand; }
        //    set { SetValue(ref _clearAllCommand, value); }
        //}

        internal GroupNotificationCollection GroupNotifications
            => NotificationService?.Notifications as GroupNotificationCollection;

        public ICommand ItemLoadedCommand
        {
            get { return _itemLoadedCommand; }
            set { SetValue(ref _itemLoadedCommand, value); }
        }

        //public ICommand ItemClosedCommand
        //{
        //    get { return _itemClosedCommand; }
        //    set { SetValue(ref _itemClosedCommand, value); }
        //}

        public ICommand ItemClickedCommand
        {
            get { return _itemClickedCommand; }
            set { SetValue(ref _itemClickedCommand, value); }
        }

        public INotificationInfoCollection Notifications => NotificationService?.Notifications;

        public void AddNotification(INotificationInfo notification) => Notifications.Add(notification);

        public event EventHandler<NotificationEventArgs> ItemClick;

        /// <summary>
        ///     calling by Item_SizeChanged
        /// </summary>
        /// <param name="obj"></param>
        private void OnItemClosed(object obj)
        {
            var item = obj.CastAs<NotificationItem>();
            if (item != null)
            {
                item.SizeChanged -= Item_SizeChanged;
                Notifications.Remove(item.NotificationInfo);
            }
            else
            {
                var group = obj.CastAs<GroupNotificationItem>();
                if (group == null) return;

                group.SizeChanged -= Item_SizeChanged;
                GroupNotifications.Remove(group.DataContext as GroupNotificationInfo);
            }
        }

        private void OnItemClicked(object obj)
        {
            var item = obj.CastAs<NotificationItem>();
            if (item == null) return;

            try
            {
                var e = new NotificationEventArgs(obj.CastAs<INotificationInfo>());
                ItemClick?.Invoke(this, e);
                if (e.Handled) return;
                NavigationExecuter.Execute(e.NotificationInfo.NavigationsParameters, DefaultNavigationCallback);
            }
            finally
            {
                item.RaiseCloseEvent();
            }
        }

        private void OnItemLoaded(object obj)
        {
            var item = obj.CastAs<FrameworkElement>();
            if (item == null) return;

            item.SizeChanged += Item_SizeChanged;
        }

        private void Item_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var item = (FrameworkElement) sender;
            if (item.Opacity <= 0.0 && e.NewSize.Height <= 0.0)
                OnItemClosed(sender);
        }

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = "Notification Center";
            viewHeader = viewTitle;
        }

        //private void ClearAll() => Notifications?.Clear();
        //}
        //    set.Add(item);
        //    item.SetResourceReference(FrameworkElement.StyleProperty, ResourceKeys.NotificationClearButtonStyle);
        //    var item = new NavigationItem { DataContext = this };
        //    base.GetToolbarItems(set);
        //{

        //protected override void GetToolbarItems(IToolBarSet set)
    }
}