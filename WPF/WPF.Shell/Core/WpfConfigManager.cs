#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HBD.Framework;
using HBD.Mef.Shell.Configuration;
using HBD.WPF.Shell.Configuration;
using HBD.WPF.Shell.Theme;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class WpfConfigManager : ShellConfigManager<WpfShellConfig,ModuleConfig>, IWpfConfigManager
    {
        private IList<IShellTheme> _themes;
        public IList<IShellTheme> Themes => SingletonManager.GetOrLoad(ref _themes, LoadThemes);

        private IList<IShellTheme> LoadThemes()
        {
            var list = new List<IShellTheme>();
            var path = Path.GetFullPath(ShellConfig.ThemePath);
            if (!Directory.Exists(path)) return list;

            var types = from f in Directory.GetFiles(path, "*.dll")
                let assembliy = Assembly.LoadFile(f)
                from t in assembliy.GetExportedTypes()
                where typeof(IShellTheme).IsAssignableFrom(t)
                select t;

            list.AddRange(types.Select(t => (IShellTheme) t.CreateInstance()));

            return list;
        }
    }
}