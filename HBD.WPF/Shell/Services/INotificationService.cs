#region

using System;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Services
{
    public interface INotificationService
    {
        INotificationInfoCollection Notifications { get; }

        void Notify(string title, string message, bool keepInCentral = true);

        void Notify(string title, string message, object iconUri, bool keepInCentral = true);

        void Notify(string title, string message, NotificationIconType iconType, bool keepInCentral = true);

        void Notify(INotificationInfo message);

        void Notify(Action<INotificationInfo> message);
    }
}