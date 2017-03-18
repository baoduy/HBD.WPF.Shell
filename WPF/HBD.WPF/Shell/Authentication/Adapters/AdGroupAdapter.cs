using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Data;
using HBD.Framework;
using HBD.Mef.Logging;

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public class AdGroupAdapter : AuthenticationAdapterBase<AdGroupValidationInfo>
    {
        private string[] _groups;

        public string[] Groups => SingletonManager.GetOrLoad(ref _groups, GetGroups);
        protected virtual string[] GetGroups()
        {
            try
            {
                //If computer is not joined to any Domain then return null.
                if (Environment.UserDomainName.EqualsIgnoreCase(Environment.MachineName))
                {
                    Logger?.Warn($"AdGroupAdapter: This PC {Environment.MachineName} is not joined to any Domain.");
                    return null;
                }

                if (AuthenticationService == null)
                {
                    Logger?.Warn("AdGroupAdapter: This AuthenticationService is not found.");
                    return null;
                }

                using (var domain = new PrincipalContext(ContextType.Domain))
                {
                    using (var user = UserPrincipal.FindByIdentity(domain, AuthenticationService.UserName))
                    {
                        var groups = user?.GetAuthorizationGroups().OfType<GroupPrincipal>()
                            .Where(g => g.GroupScope == GroupScope.Global && g.IsSecurityGroup == true).ToList();

                        return groups?.Select(principal => principal.Name).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Exception(ex);
                return null;
            }
        }

        public override bool Validate(AdGroupValidationInfo validationInfo)
        {
            if (validationInfo.Groups.NotAnyItem())
                throw new ValueUnavailableException("Groups of AdGroupValidationInfo");
            return Groups?.Any(g => validationInfo.Groups.AnyIgnoreCase(g)) == true;
        }

        public override void Dispose()
        {
        }
    }
}
