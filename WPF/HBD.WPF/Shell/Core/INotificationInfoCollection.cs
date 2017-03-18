#region

using System.Collections.Specialized;
using System.ComponentModel;

#endregion

namespace HBD.WPF.Shell.Core
{
    public interface INotificationInfoCollection : INotifyCollectionChanged, INotifyPropertyChanged
    {
        int Count { get; }
        void Add(INotificationInfo item);

        void Insert(int index, INotificationInfo item);

        bool Remove(INotificationInfo item);

        void Clear();
    }
}