#region

using System.ComponentModel.Composition;
using HBD.WPF.ShellOptionManager.Module.ViewModels;

#endregion

namespace HBD.WPF.ShellOptionManager.Module.Views
{
    [Export]
    public partial class ShellOptionView
    {
        public ShellOptionView()
        {
            InitializeComponent();
        }

        [Import]
        public ShellOptionViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}