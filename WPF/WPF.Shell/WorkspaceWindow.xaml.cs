#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.Shell
{
    [Export(typeof(WorkspaceWindow))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WorkspaceWindow
    {
        public WorkspaceWindow()
        {
            InitializeComponent();
        }

        [Import]
        public IShellViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}