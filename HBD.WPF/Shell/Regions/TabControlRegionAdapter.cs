using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Prism.Regions;

namespace HBD.WPF.Shell.Regions
{
    public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
    {
        internal TabControl RegionTarget { get; private set; }

        public TabControlRegionAdapter(IRegionBehaviorFactory factory)
       : base(factory)
        {

        }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            this.RegionTarget = regionTarget;

            region.Views.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var view = e.NewItems.OfType<FrameworkElement>().First();
                            regionTarget.Items.Add(view);
                            regionTarget.SelectedItem = view;
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        {
                            var view = e.OldItems.OfType<FrameworkElement>().First();
                            regionTarget.Items.Remove(view);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        break;
                }
            };
        }

        protected override IRegion CreateRegion() => new TabActiveRegion(this);
    }
}
