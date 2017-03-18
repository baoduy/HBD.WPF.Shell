#region

using HBD.WPF.Shell.Navigation;

#endregion

namespace HBD.WPF.Shell.Core
{
    /// <summary>
    ///     This interface is available for ViewModel only.
    /// </summary>
    public interface IToolbarInfo
    {
        IToolBarSet ToolbarItems { get; }
    }
}