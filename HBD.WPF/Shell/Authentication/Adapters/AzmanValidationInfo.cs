#region

using HBD.Framework;
using HBD.Mef.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public class AzmanValidationInfo : IRoleValidationInfo, IGroupValidationInfo, IScopeValidationInfo
    {
        /// <summary>
        ///     The list of Users groups need to be validates.
        /// </summary>
        public string[] Groups { get; set; }

        /// <summary>
        ///     The list of Users scopes need to be validates.
        /// </summary>
        public string[] Scopes { get; set; }

        /// <summary>
        ///     The list of Roles need to be validate.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        ///     The list of Users operations need to be validates.
        /// </summary>
        public string[] Operations { get; set; }

        public bool IsEmpty()
            => Groups.NotAnyItem() && Scopes.NotAnyItem() && Operations.NotAnyItem() && Roles.NotAnyItem();
    }
}