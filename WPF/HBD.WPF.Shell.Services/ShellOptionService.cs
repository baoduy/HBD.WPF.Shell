#region

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Instrumentation;
using System.Windows;
using System.Windows.Controls;
using HBD.Framework.Core;
using HBD.Mef;
using HBD.WPF.Shell.Configuration;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(IShellOptionService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellOptionService : NotifyPropertyChange, IShellOptionService
    {
        private double _height = 600;
        private bool _isSettingLoaded;
        private double _left;
        private Dock _tabStripPlacement = Dock.Bottom;
        private string _theme;
        private double _top;
        private bool _topmost;
        private double _width = 800;
        private WindowState _windowState = WindowState.Maximized;
        private WindowStyle _windowStyle = WindowStyle.SingleBorderWindow;
        protected virtual string SettingFileName { get; } = "Setting.json";

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IWpfConfigManager ShellConfigManager { protected get; set; }

        public WindowState WindowState
        {
            get { return _windowState; }
            set { SetValue(ref _windowState, value); }
        }

        [DefaultValue("")]
        public string Theme
        {
            get { return _theme; }
            set { SetValue(ref _theme, value); }
        }

        [DefaultValue(Dock.Bottom)]
        public Dock TabStripPlacement
        {
            get { return _tabStripPlacement; }
            set { SetValue(ref _tabStripPlacement, value); }
        }

        public double Width
        {
            get { return _width; }
            set { SetValue(ref _width, value); }
        }

        public double Height
        {
            get { return _height; }
            set { SetValue(ref _height, value); }
        }

        public double Left
        {
            get { return _left; }
            set { SetValue(ref _left, value); }
        }

        public double Top
        {
            get { return _top; }
            set { SetValue(ref _top, value); }
        }

        public bool Topmost
        {
            get { return _topmost; }
            set { SetValue(ref _topmost, value); }
        }

        public WindowStyle WindowStyle
        {
            get { return _windowStyle; }
            set { SetValue(ref _windowStyle, value); }
        }

        public void LoadSetting()
        {
            if (_isSettingLoaded) return;
            _isSettingLoaded = true;
            //Load setting here
            JsonConfigHelper.ReadConfig(this, SettingFileName);
        }

        public void SaveSetting() => JsonConfigHelper.SaveConfig(this, SettingFileName);

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == ExtractPropertyName(() => Theme))
            {
                //Apply New Theme
                var theme = ShellConfigManager.Themes.FirstOrDefault(t => t.Name == Theme) ??
                            ShellConfigManager.Themes.FirstOrDefault();

                if (theme != null)
                {
                    Application.Current.Resources.MergedDictionaries.Add(theme.Resource);
                    _theme = theme.Name;
                }
                else throw new InstanceNotFoundException(Theme);
            }
        }
    }
}