#region

using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using HBD.WPF;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace WPF.Demo.Module
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ViewModel1 : ViewModelBase
    {
        private ICommand _notifyCommand;

        public ViewModel1()
        {
            NotifyCommand = new ActionCommand(SendNotify);
        }

        public ICommand NotifyCommand
        {
            get { return _notifyCommand; }
            set { SetValue(ref _notifyCommand, value); }
        }

        protected override void GetToolbarItems(IToolBarSet set)
        {
            base.GetToolbarItems(set);
            set.Add(DefaultToolBarItem.Add, new ActionCommand(AddNew));
            set.Add(DefaultToolBarItem.Edit, new ActionCommand(Edit));
            set.Add(DefaultToolBarItem.Remove, new ActionCommand(ThrowException));
            set.Add(DefaultToolBarItem.Refresh, new ActionCommand(EditDialog));
            set.Add("Show/Hide Busy Dialog", new ActionCommand(ShowHideBusyDialod));
            set.AddSeparator();
        }

        private void ShowHideBusyDialod()
        {
            this.ShowBusy("busy and loading...");
        }

        private void SendNotify()
        {
            NotificationService.Notify("Notification Header",
                "That works fine, but what I am trying to do is dynamically change the colors. With your solution I am still using a static resource. Can I bind the R, G, B to anything?",
                NotificationIconType.Info);

            NotificationService.Notify(n =>
            {
                n.Title = "Alert";
                n.Message = "That works fine, but what I am trying to do is dynamically change the colors.";
                n.IconType = NotificationIconType.Alert;
                n.IsKeepInCentral = true;
                n.CreatedDate = DateTime.Today.AddDays(-2);
                //n.AddCommandNavigate(NotifyClicked);
                n.GroupTitle = "A";
            });

            NotificationService.Notify(n =>
            {
                n.Title = "Alert";
                n.Message = "That works fine, but what I am trying to do is dynamically change the colors.";
                n.Icon = "pack://application:,,,/WPF.Demo.Module;component/Resources/Icons/pin_blue.png";
                n.IsKeepInCentral = true;
                n.CreatedDate = DateTime.Today.AddDays(-12);
                //n.AddCommandNavigate(NotifyClicked);
                n.GroupTitle = "B";
            });

            StatusService.SetStatus("Hello From Demo");
        }

        private void NotifyClicked() => MessageBox.Show("Notification Clicked");

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = "View 1";
            viewHeader = "View 1 Header";
        }

        private void EditDialog()
        {
            DialogService.ShowDialog<ColorViewer>(this, "AA", null);
        }

        private void ThrowException()
        {
            //DialogService.Info(this, "Hello");
            //DialogService.Alert(this, "Hello");
            MessageBoxService.Confirm(this, "Hello");
        }

        private void AddNew()
        {
            //this.SetBusy(true, "Loading...");
            DialogService.ShowDialog<ColorViewer>(this, p =>
            {
                p.Title = "AA";
                p.DialogType = DialogType.Dialog;
            }, null);
            //this.SetBusy(false);
        }

        private void Edit()
        {
            this.ShowBusy("Loading...");
            DialogService.ShowDialog<ColorViewer>(this, "AA", DialogButtons.None);
            //this.SetBusy(false);
        }
    }
}