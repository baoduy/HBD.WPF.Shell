#region

using System.Collections.Specialized;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Controls
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        public TabControl()
        {
            var view = CollectionViewSource.GetDefaultView(Items);
            view.CollectionChanged += (o, e) => OnTabItemCollectionChanged(e);
        }

        public event NotifyCollectionChangedEventHandler TabItemCollectionChanged;

        protected virtual void OnTabItemCollectionChanged(NotifyCollectionChangedEventArgs e)
            => TabItemCollectionChanged?.Invoke(this, e);
    }
}