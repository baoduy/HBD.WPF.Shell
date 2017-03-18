#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using HBD.Mef.Shell.Navigation;

#endregion

namespace HBD.WPF.Shell.Core
{
    public interface INotificationInfo : INotifyPropertyChanged
    {
        string Message { get; set; }
        Guid Id { get; }
        object Icon { get; set; }
        string Title { get; set; }
        DateTime CreatedDate { get; set; }
        string FormatedCreatedDate { get; }
        NotificationIconType IconType { get; set; }

        /// <summary>
        ///     If the Group title of notification is empty. It will be added to the default group in the
        ///     notification center.
        /// </summary>
        string GroupTitle { get; set; }

        /// <summary>
        ///     Indicate to keep this notification in the center.
        /// </summary>
        bool IsKeepInCentral { get; set; }

        IList<INavigationParameter> NavigationsParameters { get; }
    }
}