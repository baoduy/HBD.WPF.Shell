#region

using System.ComponentModel;
using HBD.Mef.Shell.Configuration;

#endregion

namespace HBD.WPF.Shell.Configuration
{
    public sealed class WpfShellConfig : ShellConfig
    {
        private string _defaultTheme = "Default";
        private string _environmentDescription;

        public string EnvironmentDescription
        {
            get { return _environmentDescription; }
            set { SetValue(ref _environmentDescription, value); }
        }

        /// <summary>
        ///     Default Theme Name
        /// </summary>
        [DefaultValue("Default")]
        public string DefaultTheme
        {
            get { return _defaultTheme; }
            set { SetValue(ref _defaultTheme, value); }
        }
    }
}