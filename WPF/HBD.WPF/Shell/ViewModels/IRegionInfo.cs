#region

using System.Windows.Input;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    public interface IRegionInfo
    {
        RegionInfoItem RegionInfo { get; }

        ICommand CloseCommand { get; }

        void Close();
    }
}