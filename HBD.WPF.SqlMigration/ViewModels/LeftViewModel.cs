#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.SqlMigration.Module.ViewModels
{
    [Export]
    public class LeftViewModel : ViewModelBase
    {
        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = viewHeader = "Left Menu";
        }

        protected override void GetToolbarItems(IToolBarSet set)
        {
            base.GetToolbarItems(set);
            //set.Add(DefaultToolBarItem.Add, null, "Add");
        }
    }
}