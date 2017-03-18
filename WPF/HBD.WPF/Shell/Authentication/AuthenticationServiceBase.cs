#region

using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Instrumentation;
using HBD.Framework;
using HBD.Framework.Core;
using HBD.Mef.Shell.Core;
using HBD.WPF.Shell.Authentication.Adapters;

#endregion

namespace HBD.WPF.Shell.Authentication
{
    public abstract class AuthenticationServiceBase : NotifyPropertyChange, IAuthenticationService
    {
        private string _userName;

        [ImportMany(AllowRecomposition = true)]
        public AdapterCollection Adapters { get; } = new AdapterCollection();

        public virtual string UserName
        {
            get { return _userName; }
            protected set
            {
                SetValue(ref _userName, value);
                RaisePropertyChanged(() => UserNameWithoutDomain);
            }
        }

        public virtual string UserNameWithoutDomain => UserPrincipalHelper.GetUserNameWithoutDomain(UserName);
        public virtual bool IsAuthenticated => UserName.IsNullOrEmpty();

        public bool IsValid(IPermissionValidationInfo info)
        {
            if (info == null) return true;
            if (!IsAuthenticated) return false;

            var adapters = this.Adapters.Where(a => a.IsAdaptFor(info)).ToList();
            if (adapters.Count <= 0)
                throw new InstanceNotFoundException($"Authentication Adapter for {info.GetType().FullName} is not found.");

            return adapters.Any(a => a.Validate(info));
        }

        public abstract bool Login();
        //public abstract Task<bool> LoginAsync();

        public abstract void LogOut();

        //public abstract Task LogOutAsync();

        public abstract void Dispose();
    }
}