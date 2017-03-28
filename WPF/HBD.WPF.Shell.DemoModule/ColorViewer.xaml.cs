#region

using System.ComponentModel.Composition;
using System.Windows.Controls;

#endregion

namespace WPF.Demo.Module
{
    /// <summary>
    ///     Interaction logic for View2.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ColorViewer : UserControl
    {
        public ColorViewer()
        {
            InitializeComponent();
        }

        [Import]
        public ColorViewerViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}