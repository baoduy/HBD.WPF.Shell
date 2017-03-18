#region

using System.ComponentModel.Composition;
using HBD.Mef.Shell.Views;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellStatusView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ShellStatusView : IShellStatusView
    {
        public ShellStatusView()
        {
            InitializeComponent();
        }

        [Import]
        public IShellStatusViewModel ViewModel
        {
            //private get { return DataContext as IShellStatusViewModel; }
            set { DataContext = value; }
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)

        //{
        //    base.OnRenderSizeChanged(sizeInfo);
        //    this.ViewModel.ActualWidth = this.ActualWidth;
        //}
    }
}