#region

using System;
using System.Collections.ObjectModel;

#endregion

namespace HBD.WPF.Shell.Authentication.Adapters
{
    public class AdapterCollection : Collection<IAuthenticationAdapter>, IDisposable
    {
        public void Dispose()
        {
            foreach (var p in this)
                p.Dispose();
            Clear();
        }
    }
}