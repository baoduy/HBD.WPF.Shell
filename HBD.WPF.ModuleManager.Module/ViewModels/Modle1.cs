#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.ModuleManager.Module.ViewModels
{
    [Export]
    public class Modle1 : ViewModelBase
    {
        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = viewHeader = "1aaaaa";
        }
    }
}