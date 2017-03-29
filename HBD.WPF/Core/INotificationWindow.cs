#region

using System;
using System.Windows.Input;
using HBD.WPF.EventArguments;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Core
{
    public interface INotificationWindow
    {
        ICommand ItemClickedCommand { get; }

        //ICommand ItemClosedCommand { get; }
        ICommand ItemLoadedCommand { get; }

        INotificationInfoCollection Notifications { get; }

        void AddNotification(INotificationInfo notification);

        event EventHandler<NotificationEventArgs> ItemClick;
    }
}