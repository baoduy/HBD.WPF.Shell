using HBD.Framework.Attributes;
using HBD.Framework.Core;
using Prism.Regions;

namespace HBD.WPF.Shell.Regions
{
    public class TabActiveRegion : AllActiveRegion
    {
        private readonly TabControlRegionAdapter _regionAdapter;

        public TabActiveRegion([NotNull]TabControlRegionAdapter regionAdapter)
        {
            Guard.ArgumentIsNotNull(regionAdapter, nameof(regionAdapter));
            _regionAdapter = regionAdapter;
        }

        public override void Activate(object view)
        {
            base.Activate(view);

            if (_regionAdapter.RegionTarget != null)
                _regionAdapter.RegionTarget.SelectedItem = view;
        }
    }
}
