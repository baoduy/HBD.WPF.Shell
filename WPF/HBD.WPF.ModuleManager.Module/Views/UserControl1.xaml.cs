#region

using System.ComponentModel.Composition;
using HBD.WPF.ModuleManager.Module.ViewModels;

#endregion

namespace HBD.WPF.ModuleManager.Module.Views
{
    [Export]
    public partial class UserControl1
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        [Import]
        public Modle1 ViewModel
        {
            set { DataContext = value; }
        }
    }
}