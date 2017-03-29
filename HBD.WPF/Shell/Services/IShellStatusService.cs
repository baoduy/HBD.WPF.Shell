#region

using System.ComponentModel;
using System.Windows.Media;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Services
{
    public interface IShellStatusService : Mef.Shell.Services.IShellStatusService, INotifyPropertyChanged
    {
        StatusInfo CurrentStatus { get; }

        void SetStatus(StatusInfo status);
        void SetStatus(string status, Color backgroundColor);
    }
}