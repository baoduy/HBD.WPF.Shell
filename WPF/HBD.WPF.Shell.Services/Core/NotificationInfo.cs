#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using HBD.Framework.Core;
using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Services.Core
{
    [Export(typeof(INotificationInfo))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NotificationInfo : Iconable, INotificationInfo
    {
        private DateTime _createdDate;

        private NotificationIconType _iconType = NotificationIconType.None;

        private Guid _id;

        private bool _isKeepIncentral;

        private string _message;

        private string _title;

        public NotificationInfo()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            NavigationsParameters = new List<INavigationParameter>();
        }

        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }

        public Guid Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        public override object Icon
        {
            get
            {
                switch (IconType)
                {
                    case NotificationIconType.None:
                        return string.Empty;

                    case NotificationIconType.Info:
                        return ResourceKeys.GetAppResource(ResourceKeys.InfoIcon);

                    case NotificationIconType.Alert:
                        return ResourceKeys.GetAppResource(ResourceKeys.AlertIcon);

                    case NotificationIconType.Custom:
                    default:
                        return base.Icon;
                }
            }
            set
            {
                base.Icon = value;
                if (value != null)
                    IconType = NotificationIconType.Custom;
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set
            {
                SetValue(ref _createdDate, value);
                RaisePropertyChanged(() => FormatedCreatedDate);
            }
        }

        public string FormatedCreatedDate
        {
            get
            {
                if (CreatedDate >= DateTime.Today)
                    return CreatedDate.ToString("HH:mm");
                if (DateTimeFormatInfo.CurrentInfo == null) return CreatedDate.ToShortDateString();

                var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
                var calRule = DateTimeFormatInfo.CurrentInfo.CalendarWeekRule;
                var dow = DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

                return cal.GetWeekOfYear(CreatedDate, calRule, dow) == cal.GetWeekOfYear(DateTime.Today, calRule, dow)
                    ? CreatedDate.ToString("ddd")
                    : CreatedDate.ToString("dd.MMM");
            }
        }

        public NotificationIconType IconType
        {
            get { return _iconType; }
            set { SetValue(ref _iconType, value); }
        }

        /// <summary>
        ///     If the Group title of notification is empty. It will be added to the default group in the
        ///     notification center.
        /// </summary>
        public string GroupTitle { get; set; }

        /// <summary>
        ///     Indicate to keep this notification in the center.
        /// </summary>
        public bool IsKeepInCentral
        {
            get { return _isKeepIncentral; }
            set { SetValue(ref _isKeepIncentral, value); }
        }

        public IList<INavigationParameter> NavigationsParameters { get; }
    }
}