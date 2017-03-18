#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.SqlMigration.Module.ViewModels
{
    [Export]
    public class MigrationScriptViewModel : ViewModelBase
    {
        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = "Sql Migration Script";
            viewHeader = "Sql Migration Script Generation";
        }

        protected override void GetToolbarItems(IToolBarSet set)
        {
            base.GetToolbarItems(set);
            set.Add(DefaultToolBarItem.Add, new ActionCommand(AddNewData), "Add New Data table");
            set.Add(DefaultToolBarItem.Edit, new ActionCommand(EditData), "Edit Data table");
        }

        private void EditData()
        {
            //RequestNavigate("LeftRegion", typeof(LeftView).FullName);
        }

        public void AddNewData()
        {
            MessageBoxService.Info(this, "Add New data");
        }
    }
}