#region

using System;
using System.ComponentModel.Composition;
using System.Windows;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Windows;
using Microsoft.Practices.ServiceLocation;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(INotificationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NotificationService : INotificationService
    {
        private readonly NotificationWindow _growlNotifications;

        public NotificationService()
        {
            Notifications = new GroupNotificationCollection();

            _growlNotifications = new NotificationWindow();
            _growlNotifications.SetResourceReference(FrameworkElement.StyleProperty, "NotificationWindowStyle");
            _growlNotifications.ItemClick += (s, e) =>
            {
                NavigationExecuter.Execute(e.NotificationInfo.NavigationsParameters, DefaultNavigationCallback);
                Notifications.Remove(e.NotificationInfo);
            };
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IServiceLocator Container { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IMessageBoxService MessageBoxService { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public INavigationParameterExecuter NavigationExecuter { protected get; set; }

        public INotificationInfoCollection Notifications { get; }

        public void Notify(Action<INotificationInfo> message)
        {
            var item = Container.GetInstance<INotificationInfo>();
            message?.Invoke(item);
            Notify(item);
        }

        public void Notify(INotificationInfo message)
        {
            _growlNotifications.AddNotification(message);
            if (!message.IsKeepInCentral) return;

            Notifications.Insert(0, message);
        }

        public void Notify(string title, string message, bool keepInCentral = true)
            => Notify(i =>
            {
                i.Title = title;
                i.Message = message;
                i.IsKeepInCentral = keepInCentral;
            });

        public void Notify(string title, string message, NotificationIconType iconType, bool keepInCentral = true)
            => Notify(i =>
            {
                i.Title = title;
                i.Message = message;
                i.IconType = iconType;
                i.IsKeepInCentral = keepInCentral;
            });

        public void Notify(string title, string message, object iconUri, bool keepInCentral = true)
            => Notify(i =>
            {
                i.Title = title;
                i.Message = message;
                i.Icon = iconUri;
                i.IsKeepInCentral = keepInCentral;
            });

        protected virtual void DefaultNavigationCallback(NavigationResult result)
        {
            if (result.Error != null)
                MessageBoxService.Alert(this, result.Error.Message);
        }
    }
}