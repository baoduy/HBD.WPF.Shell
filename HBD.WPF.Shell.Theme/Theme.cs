#region

using System;
using System.Windows;

#endregion

namespace HBD.WPF.Shell.Theme
{
    public class Theme : IShellTheme
    {
        public string Name => "Default";

        public ResourceDictionary Resource { get; } = new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/HBD.WPF.Shell.Theme;component/Default.xaml")
        };
    }
}