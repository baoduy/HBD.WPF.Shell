#region

using System;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.EventArguments
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(INotificationInfo notificationInfo)
        {
            NotificationInfo = notificationInfo;
        }

        public INotificationInfo NotificationInfo { get; }
        public bool Handled { get; set; } = false;
    }
}