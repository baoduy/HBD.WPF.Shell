#region

using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    /// <summary>
    ///     This interface will ask RegionManager to create new RegionManager for current VIewModel. The
    ///     ScopedRegionNavigationContentLoader need to be registered into the Root RegionManager. Using
    ///     BootStrap to register ScopedRegionNavigationContentLoader.
    /// </summary>
    public interface IScopeRegion
    {
        bool CreateScopeRegionManager { get; }
        IRegionManager ScopeRegionManager { get; set; }
    }
}