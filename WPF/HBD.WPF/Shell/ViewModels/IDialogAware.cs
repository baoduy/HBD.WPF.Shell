#region

using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    /// <summary>
    ///     This interface is available for ViewModel only. This interface will be call when ViewModel is
    ///     being display as Dialog.
    /// </summary>
    public interface IDialogAware
    {
        void DialogActivating(object parameters, DialogCommandCollection commands);

        void DialogClosing(IDialogClosingResult result);

        void DialogClosed(IDialogResult result);
    }
}