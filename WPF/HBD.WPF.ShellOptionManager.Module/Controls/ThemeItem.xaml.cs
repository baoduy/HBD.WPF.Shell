#region

using System.Windows;
using HBD.WPF.Shell.Theme;

#endregion

namespace HBD.WPF.ShellOptionManager.Module.Controls
{
    public partial class ThemeItem
    {
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register("Theme",
            typeof(IShellTheme), typeof(ThemeItem));

        public ThemeItem()
        {
            InitializeComponent();
        }

        public IShellTheme Theme
        {
            get { return GetValue(ThemeProperty) as IShellTheme; }
            set
            {
                SetValue(ThemeProperty, value);
                Resources.Clear();
                Resources.MergedDictionaries.Add(value.Resource);
            }
        }
    }
}