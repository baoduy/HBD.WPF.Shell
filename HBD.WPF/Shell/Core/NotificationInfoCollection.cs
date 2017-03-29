#region

using System.Collections.ObjectModel;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class NotificationInfoCollection : ObservableCollection<INotificationInfo>, INotificationInfoCollection
    {
    }
}