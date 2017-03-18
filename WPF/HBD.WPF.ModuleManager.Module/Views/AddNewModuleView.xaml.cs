#region

using System.ComponentModel.Composition;
using HBD.WPF.ModuleManager.Module.ViewModels;

#endregion

namespace HBD.WPF.ModuleManager.Module.Views
{
    [Export]
    public partial class AddNewModuleView
    {
        public AddNewModuleView()
        {
            InitializeComponent();
        }

        [Import]
        public AddNewModuleModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}