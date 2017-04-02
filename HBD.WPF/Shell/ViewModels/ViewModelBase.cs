#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HBD.Framework;
using HBD.Framework.Core;
using HBD.Mef.Logging;
using HBD.WPF.Common;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.Regions;
using HBD.WPF.Shell.Services;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Practices.ServiceLocation;
using Prism.Regions;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    public abstract class ViewModelBase : NotifyPropertyChange, IViewModel
    {
        private bool _isLoaded;

        private INotificationService _notificationService;

        private IShellStatusService _statusService;

        protected ViewModelBase()
        {
            CloseCommand = new ActionCommand(Close);
            ValidationErrors = new List<ValidationResult>();
        }

        private IList<ValidationResult> ValidationErrors { get; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IServiceLocator Container { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IDialogService DialogService { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IMessageBoxService MessageBoxService { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellRegionNavigationService RegionNavigationService { protected get; set; }

        //Should not use IRegionManager using IShellRegionNavigationService instead.
        //[Import(AllowDefault = true, AllowRecomposition = true)]
        //public IRegionManager RegionManager { protected get; set; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public INotificationService NotificationService
        {
            protected get { return _notificationService; }
            set { SetValue(ref _notificationService, value); }
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellStatusService StatusService
        {
            protected get { return _statusService; }
            set { SetValue(ref _statusService, value); }
        }

        //[Import(AllowDefault = true)]
        //public IAuthenticationService AuthenticationService { protected get; set; }

        #region Protected Methods

        protected virtual object GetResource(string key)
        {
            if (key.IsNullOrEmpty()) return null;
            try
            {
                return Application.Current.Resources[key] ?? this.GetResourceDictionary()?[key];
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
            return null;
        }

        #endregion Protected Methods

        protected virtual void ReEvaluateCommands()
            => CommandManager.InvalidateRequerySuggested();

        //protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);

        //    if (e.PropertyName == ExtractPropertyName(() => ViewHeader))
        //        RegionNavigationService?.SetViewTitle(ViewHeader, DefaultNavigationCallback);
        //}

        #region IBusyIndicator

        private string _busyMessage;

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetValue(ref _busyMessage, value); }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetValue(ref _isBusy, value);
                OnIsBusyChanged(EventArgs.Empty);
            }
        }

        public event EventHandler IsBusyChanged;

        protected virtual void OnIsBusyChanged(EventArgs e) => IsBusyChanged?.Invoke(this, e);

        /// <summary>
        ///     Set Busy status and add the message to the log.
        /// </summary>
        /// <param name="message"></param>
        protected void ShowBusyAndTrace(string message)
        {
            this.ShowBusy(message);
            Logger.Info(message);
        }

        #endregion IBusyIndicator

        #region IToolbarInfo

        private IToolBarSet _toolbarItems;

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IToolBarSet ToolbarItems
        {
            get { return _toolbarItems; }
            private set { SetValue(ref _toolbarItems, value); }
        }

        /// <summary>
        ///     this method will call GetToolbarItems
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        protected async Task GetToolbarItemsAsync(IToolBarSet set) => await TaskCreater.CreateTask(GetToolbarItems, set);

        protected virtual void GetToolbarItems(IToolBarSet set)
        {
        }

        #endregion IToolbarInfo

        #region IViewTitle

        private string _viewTitle;

        public string ViewTitle
        {
            get { return _viewTitle; }
            set { SetValue(ref _viewTitle, value); }
        }

        private string _viewHeader;

        public string ViewHeader
        {
            get { return _viewHeader; }
            set { SetValue(ref _viewHeader, value); }
        }

        protected abstract void SetViewTitle(out string viewTitle, out string viewHeader);

        #endregion IViewTitle

        #region IActiveAware

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetValue(ref _isActive, value);
                OnActiveChanged();
            }
        }

        public event EventHandler IsActiveChanged;

        protected virtual async void OnActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
            if (_isLoaded) return;
            _isLoaded = true;

            SetViewTitle(out _viewTitle, out _viewHeader);
            RaisePropertyChanged(() => ViewTitle);
            RaisePropertyChanged(() => ViewHeader);

            this.ShowBusy("Initializing...");
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
            finally
            {
                this.HideBusy();
            }
        }

        /// <summary>
        ///     This method will call Initialize
        /// </summary>
        /// <returns></returns>
        protected virtual async Task InitializeAsync()
        {
            await GetToolbarItemsAsync(ToolbarItems);
            Initialize();
        }

        /// <summary>
        ///     This method is being called by InitializeAsync no need to implement the async any more.
        ///     If the async is needed. Override the InitializeAsync instead.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        #endregion IActiveAware

        #region IDataErrorInfo

        public string Error => string.Join(Environment.NewLine, ValidationErrors.Select(c => c.ErrorMessage));

        public string this[string columnName]
            => ValidationErrors.FirstOrDefault(c => c.MemberNames.Contains(columnName))?.ErrorMessage;

        #endregion IDataErrorInfo

        #region Validations

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }

        protected virtual ValidationContext CreateValidateContext() => new ValidationContext(this);

        protected void Validate()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, CreateValidateContext(), results);
            ValidationErrors.AddRange(results.Where(r => r != ValidationResult.Success));
        }

        #endregion Validations

        #region INavigationAware

        /// <summary>
        ///     The IRegionMemberLifetime interface defines a single read-only property, KeepAlive. If
        ///     this property returns false, the view is removed from the region when it is deactivated.
        /// </summary>
        public virtual bool KeepAlive => false;

        /// <summary>
        ///     Default return value is False If the IsNavigationTarget method always returns true,
        ///     regardless of the navigation parameters, that view instance will always be re-used. This
        ///     allows you to ensure that only one view of a particular type will be displayed in a
        ///     particular region. https://msdn.microsoft.com/en-us/library/gg430861(v=pandp.40).aspx
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext,
            Action<bool> continuationCallback)
        {
        }

        /// <summary>
        ///     User this as Default NavigationCallback. Override this for custom handle logic.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void DefaultNavigationCallback(NavigationResult result)
        {
            if (result.Error == null) return;
            Logger.Exception(result.Error);
            MessageBoxService?.Alert(this, result.Error.Message);
        }

        #endregion INavigationAware

        #region IRegionInfo

        //private RegionInfoItem _regionInfo;

        //public RegionInfoItem RegionInfo => _regionInfo ?? (_regionInfo = RegionManager?.FindRegionAndViewByModel(this));

        private ICommand _closeCommand;

        /// <summary>
        ///     Close and Deactivate this View
        /// </summary>
        public ICommand CloseCommand
        {
            get { return _closeCommand; }
            set { SetValue(ref _closeCommand, value); }
        }

        /// <summary>
        ///     Close and Deactivate this View
        /// </summary>
        public virtual void Close()
        {
            IsActive = false;
            RegionNavigationService.Close(this);
            //if (!RegionInfo.IsEmpty() && RegionInfo.RegionName.IsNotNullOrEmpty())
            //    RegionManager?.Regions[RegionInfo.RegionName].Deactivate(RegionInfo.View);
        }

        //protected virtual void RequestNavigate(string regionName, Type viewType)
        //    => RegionManager?.RequestNavigate(regionName, viewType.FullName, DefaultNavigationCallback);

        //protected virtual void RequestNavigate(string regionName, string source)
        //    => RegionManager?.RequestNavigate(regionName, source, DefaultNavigationCallback);

        //protected virtual void RequestNavigate(string regionName, Uri uri)
        //    => RegionManager?.RequestNavigate(regionName, uri, DefaultNavigationCallback);

        #endregion IRegionInfo
    }
}