#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HBD.WPF.Shell.Core;
using Prism;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    public interface IViewModel : IViewTitle, IValidatableObject, IDataErrorInfo, IActiveAware,
        IConfirmNavigationRequest, IBusyIndicator, IToolbarInfo, IRegionMemberLifetime
    {
    }
}