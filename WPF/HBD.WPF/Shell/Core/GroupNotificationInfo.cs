#region

using System;
using HBD.Framework.Core;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class GroupNotificationInfo : NotifyPropertyChange
    {
        private Guid _id;

        private NotificationInfoCollection _notifications;

        private string _title;

        public GroupNotificationInfo()
        {
            Id = Guid.NewGuid();
            Notifications = new NotificationInfoCollection();
        }

        public Guid Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        public NotificationInfoCollection Notifications
        {
            get { return _notifications; }
            set { SetValue(ref _notifications, value); }
        }
    }
}