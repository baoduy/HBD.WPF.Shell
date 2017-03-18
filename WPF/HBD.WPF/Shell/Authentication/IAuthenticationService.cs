#region

using System;
using HBD.Mef.Shell.Core;
using HBD.WPF.Shell.Authentication.Adapters;

#endregion

namespace HBD.WPF.Shell.Authentication
{
    public interface IAuthenticationService : IDisposable
    {
        AdapterCollection Adapters { get; }
        string UserName { get; }
        string UserNameWithoutDomain { get; }
        bool IsAuthenticated { get; }

        /// <summary>
        ///     Validation permission info
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool IsValid(IPermissionValidationInfo info);

        bool Login();
        //Task<bool> LoginAsync();
        void LogOut();
        //Task LogOutAsync();
    }
}