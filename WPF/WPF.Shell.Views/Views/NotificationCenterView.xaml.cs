#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.ViewModels;
using HBD.WPF.Shell.Views;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellNotificationCenterView))]
    public partial class ShellNotificationCenterView : IShellNotificationCenterView
    {
        public ShellNotificationCenterView()
        {
            InitializeComponent();
        }

        [Import]
        public IShellNotificationCenterViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}