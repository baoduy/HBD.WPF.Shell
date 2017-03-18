#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.ViewModels;
using HBD.WPF.Shell.Views;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellTitleView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ShellTitleView : IShellTitleView
    {
        public ShellTitleView()
        {
            InitializeComponent();
        }

        [Import]
        public IShellTitleViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}