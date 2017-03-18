#region

using System.ComponentModel.Composition;
using HBD.WPF.SqlMigration.Module.ViewModels;

#endregion

namespace HBD.WPF.SqlMigration.Module.Views
{
    [Export]
    public partial class LeftView
    {
        public LeftView()
        {
            InitializeComponent();
        }

        [Import]
        public LeftViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}