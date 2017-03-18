#region

using System;
using System.ComponentModel.Composition;
using HBD.Framework.Core;
using HBD.WPF.Shell.Authentication;

#endregion

namespace HBD.WPF.WindowAuthentication.Module
{
    [Export(typeof(IAuthenticationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WindowAuthenticationService : AuthenticationServiceBase
    {
        public override string UserName => UserPrincipalHelper.UserName;

        public override bool Login()
        {
            throw new NotImplementedException();
        }

        //public override Task<bool> LoginAsync()
        //{
        //    throw new System.NotImplementedException();
        //}

        public override void LogOut()
        {
            throw new NotImplementedException();
        }

        //public override Task LogOutAsync()
        //{
        //    throw new System.NotImplementedException();
        //}

        public override void Dispose()
        {
        }
    }
}