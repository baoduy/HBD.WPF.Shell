#region

using System.Windows;

#endregion

namespace HBD.WPF.Shell.Core
{
    public interface IDialogResult
    {
        MessageBoxResult Result { get; }
        object ResultValue { get; }
    }
}