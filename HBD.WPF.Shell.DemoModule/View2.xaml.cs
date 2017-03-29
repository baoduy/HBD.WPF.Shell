#region

using System.ComponentModel.Composition;
using System.Windows.Controls;

#endregion

namespace WPF.Demo.Module
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class View2 : UserControl
    {
        public View2()
        {
            InitializeComponent();
        }
    }
}