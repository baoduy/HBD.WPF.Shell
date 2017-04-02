#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using HBD.Framework;
using HBD.Framework.Core;

#endregion

namespace HBD.WPF.Shell.UI.Common
{
    /// <summary>
    ///     The proxy ProxyObservableCollection helps to execute the CollectionChanged event when adding
    ///     item or removing item satisfy the condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProxyObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        private readonly Func<T, bool> _condition;

        public ProxyObservableCollection(ICollection<T> internaCollection, Func<T, bool> condition)
        {
            Guard.ArgumentIsNotNull(internaCollection, nameof(internaCollection));
            Guard.ArgumentIsNotNull(condition, nameof(condition));

            _condition = condition;
            var changed = internaCollection as INotifyCollectionChanged;
            if (changed != null)
                changed.CollectionChanged += _internaCollection_CollectionChanged;
            else throw new ArgumentException($"{nameof(internaCollection)} must be a INotifyCollectionChanged");
        }

        private void _internaCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.OldItems?.OfType<T>()
                .ForEach(i =>
                {
                    i.PropertyChanged -= I_PropertyChanged;
                    Remove(i);
                });

            var index = e.NewStartingIndex;

            e.NewItems?.OfType<T>().
                ForEach(i =>
                {
                    if (_condition(i))
                        if (index >= 0 && Count > index)
                            Insert(index++, i);
                        else Add(i);
                    i.PropertyChanged += I_PropertyChanged;
                });
        }

        private void I_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var t = (T) sender;

            if (!_condition(t))
                Remove(t);
            else if (!Contains(t))
                Add(t);
        }
    }
}