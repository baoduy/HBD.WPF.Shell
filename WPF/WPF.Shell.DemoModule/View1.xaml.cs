#region

using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace WPF.Demo.Module
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class View1 : UserControl
    {
        public View1()
        {
            InitializeComponent();
        }

        [Import]
        public ViewModel1 ViewModel
        {
            set { DataContext = value; }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("AAA");
        }
    }
}