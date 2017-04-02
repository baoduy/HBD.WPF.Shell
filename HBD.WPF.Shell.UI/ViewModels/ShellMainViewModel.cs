#region

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using HBD.WPF.Shell.Services;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.Shell.UI.ViewModels
{
    [Export(typeof(IShellMainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellMainViewModel : ViewModelBase, IShellMainViewModel
    {
        //private ViewColection _activatedObjects;
        private ICommand _closeViewCommand;
        private FrameworkElement _selectedElement;
        private IShellOptionService _shellOptionService;

        public ShellMainViewModel()
        {
            //ActivatedObjects = new ViewColection();
            //ActivatedObjects.CollectionChanged += ActivatedObjects_CollectionChanged;
            CloseViewCommand = new ActionCommand(CloseView);
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IShellOptionService ShellOptionService
        {
            get { return _shellOptionService; }
            set { SetValue(ref _shellOptionService, value); }
        }

        //public ViewColection ActivatedObjects
        //{
        //    get { return _activatedObjects; }
        //    private set { SetValue(ref _activatedObjects, value); }
        //}

        public ICommand CloseViewCommand
        {
            get { return _closeViewCommand; }
            set { SetValue(ref _closeViewCommand, value); }
        }

        public FrameworkElement SelectedElement
        {
            get { return _selectedElement; }
            set { SetValue(ref _selectedElement, value); }
        }

        //public override void ConfirmNavigationRequest(NavigationContext navigationContext,
        //    Action<bool> continuationCallback)
        //{
        //    var viewName = navigationContext.Uri.OriginalString;
        //    if (viewName == typeof(IShellMainView).FullName)
        //        continuationCallback(true);
        //    else
        //    {
        //        continuationCallback(false);
        //        LoadView(viewName);
        //    }
        //}

        //public override void OnNavigatedTo(NavigationContext navigationContext)
        //    => LoadView(navigationContext.Parameters[NavigaParameters.ViewName]);

        //private void ActivatedObjects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    var list = e.OldItems ?? e.NewItems;
        //    var view = list?.OfType<FrameworkElement>().FirstOrDefault();
        //    if (view == null) return;

        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //        {
        //            //Shown the View
        //            SelectedElement = view;
        //            //Active the View
        //            var act = view.CastAs<IActiveAware>();
        //            if (act != null)
        //            {
        //                act.IsActive = true;
        //                act.IsActiveChanged += Act_IsActiveChanged;
        //            }

        //            RegionNavigationService.SetViewTitle(view.CastAs<IViewTitle>()?.ViewHeader);
        //            StatusService.SetStatus($"{view.CastAs<IViewTitle>()?.ViewTitle} is ready.");
        //        }
        //            break;

        //        case NotifyCollectionChangedAction.Remove:
        //        {
        //            //Deactivate View and remove.
        //            var act = view.CastAs<IActiveAware>();
        //            if (act != null) act.IsActive = false;
        //            StatusService.SetStatus($"{view.CastAs<IViewTitle>()?.ViewTitle} is closed.");
        //        }
        //            break;
        //    }
        //}

        //private void Act_IsActiveChanged(object sender, EventArgs e)
        //{
        //    var act = sender.CastAs<IActiveAware>();

        //    if (act?.IsActive != false) return;
        //    act.IsActiveChanged -= Act_IsActiveChanged;

        //    var view = sender.CastAs<IRegionInfo>()?.RegionInfo.View ??
        //               ActivatedObjects.FirstOrDefault(a => a.DataContext == sender);

        //    if (view != null)
        //        CloseView(view);
        //}

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
            => viewTitle = viewHeader = string.Empty;

        //private void LoadView(object viewType)
        //{
        //    //If the view already existed then select the view.
        //    var info = ActivatedObjects.TryGet(viewType);
        //    if (info.Value != null)
        //    {
        //        SelectedElement = info.Value;
        //        return;
        //    }

        //    if (info.Key.IsNullOrEmpty()) return;

        //    var view = Container.GetInstance<object>(info.Key) as FrameworkElement;
        //    if (view == null) return;

        //    //Add and selected the view.
        //    ActivatedObjects.Add(view);
        //}

        private void CloseView(object viewObj)
        {
            var view = viewObj as FrameworkElement;
            if (view == null) return;
            RegionNavigationService.Close(view);

            //Get current index of view and selected status
            //var isSelected = ReferenceEquals(SelectedElement, view);

            //ActivatedObjects.Remove(view);

            //Select the neighbor view based on index and status if available.
            //if (ActivatedObjects.Count <= 0 || !isSelected) return;

            //SelectedElement = ActivatedObjects.LastOrDefault();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName != ExtractPropertyName(() => SelectedElement)) return;

            var vtitle = SelectedElement.CastAs<IViewTitle>();
            RegionNavigationService.SetViewTitle(vtitle?.ViewHeader);
        }
    }
}