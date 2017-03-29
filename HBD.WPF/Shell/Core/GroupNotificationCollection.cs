#region

using System;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class GroupNotificationCollection : ObservableCollection<GroupNotificationInfo>, INotificationInfoCollection
    {
        public void Add(INotificationInfo item)
        {
            var group = GetOrAddGroup(item);
            group?.Notifications.Add(item);
        }

        public void Insert(int index, INotificationInfo item)
        {
            var group = GetOrAddGroup(item);
            group?.Notifications.Insert(index, item);
        }

        public bool Remove(INotificationInfo item)
        {
            var group = GetOrAddGroup(item);
            if (group == null) return false;
            var val = group.Notifications.Remove(item);

            if (group.Notifications.Count == 0)
                Remove(group);

            return val;
        }

        private GroupNotificationInfo GetOrAddGroup(INotificationInfo item)
        {
            if (item == null) return null;
            var group =
                this.FirstOrDefault(
                    g => string.Compare(g.Title, item.GroupTitle, StringComparison.OrdinalIgnoreCase) == 0);
            if (group != null) return group;

            group = new GroupNotificationInfo {Title = item.GroupTitle};
            Add(group);
            return group;
        }

        public virtual GroupNotificationInfo GetGroupInfo(INotificationInfo item)
            => GetOrAddGroup(item);
    }
}