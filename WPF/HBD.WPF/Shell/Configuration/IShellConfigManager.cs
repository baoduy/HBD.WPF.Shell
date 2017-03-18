#region

using System.Collections.Generic;
using HBD.Mef.Shell.Configuration;
using HBD.WPF.Shell.Theme;

#endregion

namespace HBD.WPF.Shell.Configuration
{
    public interface IWpfConfigManager : IShellConfigManager<WpfShellConfig, ModuleConfig>
    {
        IList<IShellTheme> Themes { get; }
    }
}