#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.Views;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellExceptionView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ExceptionView : IShellExceptionView
    {
        public ExceptionView()
        {
            InitializeComponent();
        }
    }
}