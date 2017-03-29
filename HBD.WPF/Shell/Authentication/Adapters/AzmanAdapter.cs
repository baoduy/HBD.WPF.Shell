using System;
using System.Linq;
using System.Windows.Data;
using HBD.Framework;
using HBD.Framework.Core;

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public class AzmanAdapter : AuthenticationAdapterBase<AzmanValidationInfo>
    {
        private readonly string _connectionString;
        private AzUserInfo _azUserInfo;

        public AzmanAdapter(string application, string nameOrconnectionString)
        {
            Guard.ArgumentIsNotNull(application, nameof(application));
            Guard.ArgumentIsNotNull(nameOrconnectionString, nameof(nameOrconnectionString));

            ApplicationName = application;
            _connectionString = nameOrconnectionString;
        }

        public string ApplicationName { get; }

        public AzUserInfo AzUserInfo => SingletonManager.GetOrLoad(ref _azUserInfo, GetAzUserInfo);
        protected virtual AzUserInfo GetAzUserInfo()
        {
            try
            {
                if (ApplicationName.IsNullOrEmpty())
                {
                    Logger?.Warn("AzmanAdapter: This Application Name should not be empty.");
                    return null;
                }

                if (AuthenticationService == null)
                {
                    Logger?.Warn("AzmanAdapter: This AuthenticationService is not found.");
                    return null;
                }

                using (var context = new AzManContext(_connectionString))
                {
                    var app = context.GetApplication(ApplicationName);
                    return app.GetUserInfo(AuthenticationService.UserName);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Exception(ex);
                return null;
            }
        }
        public override bool Validate(AzmanValidationInfo validationInfo)
        {
            if (validationInfo.IsEmpty())
                throw new ValueUnavailableException("AzmanAdapter: AzmanValidationInfo is empty.");

            if (validationInfo.Groups.AnyItem() && !validationInfo.Groups.All(AzUserInfo.Groups.AnyIgnoreCase))
                return false;
            if (validationInfo.Roles.AnyItem() && !validationInfo.Roles.All(AzUserInfo.Roles.AnyIgnoreCase))
                return false;
            if (validationInfo.Operations.AnyItem() && !validationInfo.Operations.All(AzUserInfo.Operations.AnyIgnoreCase))
                return false;
            if (validationInfo.Scopes.AnyItem() && !validationInfo.Scopes.All(v => AzUserInfo.UserScopeInfos.Any(s => s.ScopeName.EqualsIgnoreCase(v))))
                return false;

            return true;
        }

        public override void Dispose()
        {
        }
    }
}
