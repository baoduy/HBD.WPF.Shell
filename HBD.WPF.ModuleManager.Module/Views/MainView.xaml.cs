#region

using System.ComponentModel.Composition;
using HBD.WPF.ModuleManager.Module.ViewModels;

#endregion

namespace HBD.WPF.ModuleManager.Module.Views
{
    [Export]
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        [Import]
        public MainViewModule ViewModel
        {
            set { DataContext = value; }
        }
    }
}