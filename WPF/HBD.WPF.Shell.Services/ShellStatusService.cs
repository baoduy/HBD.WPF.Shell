#region

using System.ComponentModel.Composition;
using System.Windows.Media;
using HBD.Framework.Core;
using HBD.Mef.Shell.Core;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(IShellStatusService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellStatusService : NotifyPropertyChange, IShellStatusService
    {
        private StatusInfo _currentStatus;

        public StatusInfo CurrentStatus
        {
            get { return _currentStatus; }
            protected set { SetValue(ref _currentStatus, value); }
        }

        public void SetStatus(string status) => SetStatus(status, Colors.Transparent);

        public void SetStatus(StatusInfo status) => CurrentStatus = status;

        public void SetStatus(string status, Color backgroundColor)
            => SetStatus(new StatusInfo {Message = status, Background = new SolidColorBrush(backgroundColor)});

        public void Set(IStatusInfo info) => SetStatus(info as StatusInfo);
    }
}