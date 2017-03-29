#region

using System.ComponentModel.Composition;
using System.Windows.Controls;
using HBD.WPF.SqlMigration.Module.ViewModels;

#endregion

namespace HBD.WPF.SqlMigration.Module.Views
{
    [Export]
    public partial class MigrationScriptView : UserControl
    {
        public MigrationScriptView()
        {
            InitializeComponent();
        }

        [Import]
        public MigrationScriptViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}