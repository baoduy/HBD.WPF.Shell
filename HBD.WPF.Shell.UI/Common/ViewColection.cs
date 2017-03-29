#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HBD.Framework;
using HBD.Framework.Collections;
using HBD.Mef.Shell.Navigation;

#endregion

namespace HBD.WPF.Shell.UI.Common
{
    public class ViewColection : ICollection<FrameworkElement>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ViewColection()
        {
            InternalIndex = new List<string>();
            InternalCollection = new ObservableDictionary<string, FrameworkElement>();
            InternalCollection.CollectionChanged += InternalCollection_CollectionChanged;
        }

        private ObservableDictionary<string, FrameworkElement> InternalCollection { get; }
        private IList<string> InternalIndex { get; }

        public FrameworkElement this[string itemName] => !Contains(itemName) ? null : InternalCollection[itemName];

        public FrameworkElement this[Type itemType] => this[itemType.FullName];

        public IEnumerator<FrameworkElement> GetEnumerator() => InternalCollection.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => InternalCollection.Values.GetEnumerator();

        public void Add(FrameworkElement item)
        {
            var key = item.GetType().FullName;
            InternalCollection.Add(key, item);
            InternalIndex.Add(key);
        }

        public void Clear() => InternalCollection.Clear();

        public bool Contains(FrameworkElement item) => InternalCollection.Values.Contains(item);

        public void CopyTo(FrameworkElement[] array, int arrayIndex)
            => InternalCollection.Values.CopyTo(array, arrayIndex);

        public bool Remove(FrameworkElement item) => InternalCollection.Remove(item.GetType().FullName);

        public int Count => InternalCollection.Count;
        public bool IsReadOnly => InternalCollection.IsReadOnly;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { InternalCollection.PropertyChanged += value; }
            remove { InternalCollection.PropertyChanged -= value; }
        }

        private void InternalCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var list = e.OldItems ?? e.NewItems;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                InternalIndex.Clear();
                OnCollectionChanged(e);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removingItem = list.OfType<KeyValuePair<string, FrameworkElement>>().First();
                var index = InternalIndex.IndexOf(removingItem.Key);
                InternalIndex.RemoveAt(index);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(e.Action, removingItem.Value, index));
            }
            else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(e.Action,
                    list.OfType<KeyValuePair<string, FrameworkElement>>().Select(i => i.Value).ToList()));
        }

        public KeyValuePair<string, FrameworkElement> TryGet(object itemInfo)
        {
            var typeName = (itemInfo as Type)?.FullName ?? itemInfo as string;
            if (typeName.IsNullOrEmpty())
                typeName = (itemInfo as IViewInfo)?.ViewType.FullName ?? (itemInfo as IViewInfo)?.ViewName;

            var item = this[typeName];
            return new KeyValuePair<string, FrameworkElement>(typeName, item);
        }

        public bool Contains(Type itemType) => Contains(itemType.FullName);

        public bool Contains(string itemName) => itemName.IsNotNull() && InternalCollection.ContainsKey(itemName);

        public bool Remove(Type itemType) => Remove(itemType.FullName);

        public bool Remove(string itemName) => InternalCollection.Remove(itemName);

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            => CollectionChanged?.Invoke(this, e);
    }
}