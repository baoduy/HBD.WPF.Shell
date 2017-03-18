#region

using System.ComponentModel.Composition;
using HBD.Mef.Shell.Views;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellMenuView))]
    public partial class ShellMenuView : IShellMenuView
    {
        public ShellMenuView()
        {
            InitializeComponent();
        }

        [Import]
        public IShellMenuViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}