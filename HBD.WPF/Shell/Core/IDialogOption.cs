#region

using System.Windows;
using HBD.WPF.Core;

#endregion

namespace HBD.WPF.Shell.Core
{
    public interface IDialogOption
    {
        DialogButtons Buttons { get; set; }
        DialogCommandCollection CustomCommands { get; }
        string Title { get; set; }
        FrameworkElement View { get; set; }
        DialogType DialogType { get; set; }
        object Parameters { get; set; }
    }
}