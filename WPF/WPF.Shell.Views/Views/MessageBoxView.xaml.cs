#region

using System.ComponentModel.Composition;
using HBD.WPF.Shell.Views;

#endregion

namespace HBD.WPF.Shell.UI.Views
{
    [Export(typeof(IShellMessageBoxView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MessageBoxView : IShellMessageBoxView
    {
        public MessageBoxView()
        {
            InitializeComponent();
        }
    }
}