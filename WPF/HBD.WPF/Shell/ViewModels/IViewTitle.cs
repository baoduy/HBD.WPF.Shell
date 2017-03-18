#region

using System.ComponentModel;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    /// <summary>
    ///     This interface is available for ViewModel only.
    /// </summary>
    public interface IViewTitle : INotifyPropertyChanged
    {
        /// <summary>
        ///     View Title Will Display in the Tab Control
        /// </summary>
        string ViewTitle { get; set; }

        /// <summary>
        ///     View Header will display in the Header control.
        /// </summary>
        string ViewHeader { get; set; }
    }
}