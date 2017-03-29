#region

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HBD.Framework;
using HBD.WPF.Shell.Configuration;
using HBD.WPF.Shell.Services;
using HBD.WPF.Shell.Theme;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.ShellOptionManager.Module.ViewModels
{
    [Export]
    public class ShellOptionViewModel : ViewModelBase
    {
        private ICommand _saveCommand;
        private WindowStyle _selectedStyle;

        private Dock _selectedTabPlacement;
        private IShellTheme _selectedTheme;

        public ShellOptionViewModel()
        {
            SaveCommand = new ActionCommand(OnSave);
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IWpfConfigManager ShellConfigManager { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellOptionService ShellOptionService { protected get; set; }

        public IList<IShellTheme> Themes => ShellConfigManager.Themes;

        public IList<Dock> TabPlacementItems { get; } = new List<Dock>
        {
            Dock.Bottom,
            Dock.Left,
            Dock.Right,
            Dock.Top
        };

        public IList<WindowStyle> WindowStyles { get; } = new List<WindowStyle>
        {
            WindowStyle.None,
            WindowStyle.SingleBorderWindow,
            WindowStyle.ThreeDBorderWindow,
            WindowStyle.ToolWindow
        };

        public WpfShellConfig ShellConfig => ShellConfigManager.ShellConfig;

        public Dock SelectedTabPlacement
        {
            get { return _selectedTabPlacement; }
            set { SetValue(ref _selectedTabPlacement, value); }
        }

        public IShellTheme SelectedTheme
        {
            get { return _selectedTheme; }
            set { SetValue(ref _selectedTheme, value); }
        }

        public WindowStyle SelectedStyle
        {
            get { return _selectedStyle; }
            set { SetValue(ref _selectedStyle, value); }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand; }
            set { SetValue(ref _saveCommand, value); }
        }

        private void OnSave(object parameter)
        {
            ShellOptionService.TabStripPlacement = SelectedTabPlacement;
            ShellOptionService.Theme = SelectedTheme.Name;
            ShellOptionService.WindowStyle = SelectedStyle;

            ShellOptionService.SaveSetting();

            MessageBoxService.Info(this, "The changes had been saved.", dialogCallback: rs =>
            {
                if ((parameter as string)?.EqualsIgnoreCase("Close") == true)
                    Close();
            });
        }

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = "Options";
            viewHeader = "Option Management";
        }

        protected override void Initialize()
        {
            base.Initialize();
            //Load Data from Option Service
            SelectedTabPlacement = ShellOptionService.TabStripPlacement;
            SelectedStyle = ShellOptionService.WindowStyle;
            SelectedTheme = Themes.FirstOrDefault(t => t.Name.EqualsIgnoreCase(ShellOptionService.Theme));
        }
    }
}