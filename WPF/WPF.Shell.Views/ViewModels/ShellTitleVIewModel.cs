#region

using System;
using System.ComponentModel.Composition;
using System.Linq;
using HBD.WPF.Shell.ViewModels;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.UI.ViewModels
{
    [Export(typeof(IShellTitleViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellTitleVIewModel : ViewModelBase, IShellTitleViewModel
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        public override void ConfirmNavigationRequest(NavigationContext navigationContext,
                Action<bool> continuationCallback)
            => continuationCallback(true);

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Title = navigationContext.Parameters.FirstOrDefault().Value as string;
        }

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = viewHeader = string.Empty;
        }
    }
}