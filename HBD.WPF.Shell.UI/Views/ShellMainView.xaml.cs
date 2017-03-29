#region

using System.ComponentModel.Composition;
using HBD.Mef.Shell.Views;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellMainView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ShellMainView : IShellMainView
    {
        public ShellMainView()
        {
            InitializeComponent();
        }

        [Import]
        public IShellMainViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}