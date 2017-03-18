using System;
using HBD.Mef.Shell.Core;

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public interface IAuthenticationAdapter : IDisposable
    {
        bool IsAdaptFor(IPermissionValidationInfo validationInfo);
        bool Validate(IPermissionValidationInfo validationInfo);
    }

    public interface IAuthenticationAdapter<in TValidationInfo> : IAuthenticationAdapter where TValidationInfo : IPermissionValidationInfo
    {
        bool Validate(TValidationInfo validationInfo);
    }
}
