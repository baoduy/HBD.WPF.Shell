using HBD.Mef.Shell.Core;

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public interface IScopeValidationInfo: IPermissionValidationInfo
    {
        string[] Scopes { get; set; }
    }
}
