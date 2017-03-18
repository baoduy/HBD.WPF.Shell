using HBD.Mef.Shell.Core;

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public interface IGroupValidationInfo: IPermissionValidationInfo
    {
         string[] Groups { get; set; }
    }
}
