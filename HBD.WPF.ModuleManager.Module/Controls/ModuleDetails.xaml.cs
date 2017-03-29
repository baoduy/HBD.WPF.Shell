#region

using System.Windows;
using HBD.Mef.Shell.Configuration;

#endregion

namespace HBD.WPF.ModuleManager.Module.Controls
{
    /// <summary>
    ///     Interaction logic for ModuleDetails.xaml
    /// </summary>
    public partial class ModuleDetails
    {
        public static readonly DependencyProperty ModuleConfigProperty = DependencyProperty.Register("ModuleConfig",
            typeof(ModuleConfig), typeof(ModuleDetails));

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool), typeof(ModuleDetails));

        public ModuleDetails()
        {
            InitializeComponent();
        }

        public ModuleConfig ModuleConfig
        {
            get { return GetValue(ModuleConfigProperty) as ModuleConfig; }
            set { SetValue(ModuleConfigProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool) GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
    }
}