#region

using System.ComponentModel;

#endregion

namespace HBD.WPF.Shell.Core
{
    /// <summary>
    ///     This interface is available for ViewModel only. Allow model to send Busy message to BusyControl.
    /// </summary>
    public interface IBusyIndicator : INotifyPropertyChanged
    {
        string BusyMessage { get; set; }
        bool IsBusy { get; set; }
    }
}