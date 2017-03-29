#region

using System.ComponentModel;
using System.ComponentModel.Composition;
using HBD.WPF.Shell.Configuration;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.ViewModels;

#endregion

namespace HBD.WPF.Shell.UI.ViewModels
{
    [Export(typeof(IShellStatusViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellStatusViewModel : ViewModelBase, IShellStatusViewModel
    {
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IWpfConfigManager ShellConfigManager { get; set; }

        //private double _actualWidth = 800;
        public StatusInfo CurrentStatus => StatusService?.CurrentStatus;

        //public double ActualWidth
        //{
        //    get { return _actualWidth; }
        //    set { this.SetValue(ref _actualWidth, value); }
        //}

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = viewHeader = string.Empty;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName != ExtractPropertyName(() => StatusService)) return;
            // ReSharper disable once ExplicitCallerInfoArgument
            StatusService.PropertyChanged += (s, ev) => RaisePropertyChanged(ev.PropertyName);
        }
    }
}