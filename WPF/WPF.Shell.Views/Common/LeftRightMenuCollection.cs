#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HBD.Mef.Shell.Navigation;

#endregion

namespace HBD.WPF.Shell.UI.Common
{
    public class LeftRightMenuCollection : IMenuInfoCollection
    {
        public LeftRightMenuCollection()
        {
            InternalCollection = new MenuInfoCollection();
            LeftMenuItems = new ProxyObservableCollection<IMenuInfo>(InternalCollection,
                i => i.Alignment == MenuAlignment.Left);
            RightMenuItems = new ProxyObservableCollection<IMenuInfo>(InternalCollection,
                i => i.Alignment == MenuAlignment.Right);
        }

        private MenuInfoCollection InternalCollection { get; }

        public ObservableCollection<IMenuInfo> LeftMenuItems { get; }
        public ObservableCollection<IMenuInfo> RightMenuItems { get; }

        #region IMenuInfoCollection

        public int Count => InternalCollection.Count;

        public bool IsReadOnly => false;

        public IMenuInfo this[int index] => InternalCollection[index];

        public void Add(IMenuInfo item) => InternalCollection.Add(item);

        public void Clear() => InternalCollection.Clear();

        public bool Contains(IMenuInfo item) => InternalCollection.Contains(item);

        public void CopyTo(IMenuInfo[] array, int arrayIndex) => InternalCollection.CopyTo(array, arrayIndex);

        public IEnumerator<IMenuInfo> GetEnumerator() => InternalCollection.GetEnumerator();

        public bool Remove(IMenuInfo item) => InternalCollection.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //public int IndexOf(IMenuInfo item) => InternalCollection.IndexOf(item);

        //public void Insert(int index, IMenuInfo item) => InternalCollection.Insert(index, item);

        //public void RemoveAt(int index) => InternalCollection.RemoveAt(index);

        #endregion IMenuInfoCollection
    }
}