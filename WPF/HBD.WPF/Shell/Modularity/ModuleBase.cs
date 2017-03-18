#region

using System;
using System.Collections.Generic;
using System.Windows;
using HBD.Framework;
using HBD.Mef.Shell.Modularity;
using HBD.Mef.Shell.Navigation;
using HBD.Mef.Logging;

#endregion

namespace HBD.WPF.Shell.Modularity
{
    public abstract class ModuleBase : ShellModuleBase
    {
        protected override IEnumerable<IViewInfo> GetStartUpViewTypes()
        {
            yield break;
        }

        /// <summary>
        ///     Get Application Resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual object GetResource(string key)
        {
            if (key.IsNullOrEmpty()) return null;

            try
            {
                return Application.Current.Resources[key] ?? this.GetResourceDictionary()?[key];
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                Logger.Exception(ex);
                return null;
#endif
            }
        }
    }
}