#region

using System.Windows;

#endregion

namespace HBD.WPF.Shell.Theme
{
    public interface IShellTheme
    {
        string Name { get; }
        ResourceDictionary Resource { get; }
    }
}