using System;
using System.ComponentModel.Composition;
using HBD.Framework.Core;
using HBD.Mef.Shell.Core;
using Prism.Logging;

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public abstract class AuthenticationAdapterBase<TValidationInfo> : IAuthenticationAdapter<TValidationInfo> where TValidationInfo : class, IPermissionValidationInfo
    {
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IAuthenticationService AuthenticationService { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILoggerFacade Logger { protected get; set; }

        public abstract bool Validate(TValidationInfo validationInfo);

        public abstract void Dispose();

        public virtual bool IsAdaptFor(IPermissionValidationInfo validationInfo) => validationInfo is TValidationInfo;

        public virtual bool Validate(IPermissionValidationInfo validationInfo)
        {
            Guard.ArgumentIsNotNull(validationInfo, nameof(validationInfo));

            if (IsAdaptFor(validationInfo))
                return Validate(validationInfo as TValidationInfo);

            throw new NotSupportedException($"The Authentication Adapter {this.GetType().FullName} is not supporting {validationInfo.GetType().FullName}");
        }
    }
}
