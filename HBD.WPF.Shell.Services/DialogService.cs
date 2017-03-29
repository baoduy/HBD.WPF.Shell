#region

using System;
using System.ComponentModel.Composition;
using System.Windows;
using HBD.WPF.Controls;
using HBD.WPF.Shell.Controls;
using HBD.WPF.Windows;
using Microsoft.Practices.ServiceLocation;
using ResourceKeys = HBD.WPF.Shell.Services.Core.ResourceKeys;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(IDialogService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DialogService : DialogServiceBase
    {
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IServiceLocator Container { protected get; set; }

        protected override FrameworkElement GetView<TView>(object dataContext = null)
        {
            var view = Container.GetInstance<TView>() as FrameworkElement;
            if (view == null)
                throw new Exception($"{typeof(TView).FullName} is not found.");
            if (dataContext != null)
                view.DataContext = dataContext;
            return view;
        }

        protected override DialogWindow CreateDialogWindow(object parentViewModel, FrameworkElement contentView)
        {
            var win = new DialogWindow {Content = WrapContentView(contentView)};
            win.SetResourceReference(Window.IconProperty, ResourceKeys.DialogIcon);
            return win;
        }

        protected override ModelWindow CreateChildWindow(object parentViewModel, FrameworkElement contentView)
        {
            var win = new ModelWindow {Content = WrapContentView(contentView)};
            win.SetResourceReference(ModelWindow.IconProperty, ResourceKeys.DialogIcon);
            return win;
        }

        private static FrameworkElement WrapContentView(FrameworkElement view)
            => new ShellViewContentPresenter {Content = view};
    }
}