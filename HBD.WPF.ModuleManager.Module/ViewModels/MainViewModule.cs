#region

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HBD.Framework.EventArguments;
using HBD.Mef.Logging;
using HBD.Mef.Shell.Configuration;
using HBD.WPF.Core;
using HBD.WPF.ModuleManager.Module.Properties;
using HBD.WPF.ModuleManager.Module.Views;
using HBD.WPF.Shell.Configuration;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.ModuleManager.Module.ViewModels
{
    [Export]
    public class MainViewModule : ViewModelBase
    {
        private ICommand _addNew;
        private ICommand _save;
        private ModuleConfig _selectedModule;

        public MainViewModule()
        {
            Save = new ActionCommand(OnSave);
            AddNew = new ActionCommand(OnAddNew);
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public IWpfConfigManager ShellConfigManager { protected get; set; }

        public ModuleConfigCollection<ModuleConfig> Modules => ShellConfigManager.Modules;

        public ModuleConfig SelectedModule
        {
            get { return _selectedModule; }
            set { SetValue(ref _selectedModule, value); }
        }

        private ModuleConfig PreviousModule { get; set; }

        public ICommand Save
        {
            get { return _save; }
            set { SetValue(ref _save, value); }
        }

        public ICommand AddNew
        {
            get { return _addNew; }
            set { SetValue(ref _addNew, value); }
        }

        private void OnAddNew()
        {
            DialogService.ShowDialog<AddNewModuleView>(this, option =>
            {
                option.Title = "Add New Module";
                option.Buttons = DialogButtons.Custom;
                option.CustomCommands.Add("Cancel", MessageBoxResult.Cancel, true);
                option.DialogType = DialogType.Auto;
            });
            StatusService.SetStatus("Add New Module dialog is ready.");
        }

        private void OnSave(object parameters)
        {
            try
            {
                ShellConfigManager.SaveChanges(SelectedModule);
                MessageBoxService.Info(this, $"The changes of Module {SelectedModule.Name} had been saved.");
                StatusService.SetStatus($"Saved the changes of {SelectedModule?.Name}");
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                MessageBoxService.Alert(this, ex.Message);
            }
        }

        protected override void OnActiveChanged()
        {
            base.OnActiveChanged();
            SelectedModule = Modules.FirstOrDefault();
        }

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = Resources.ViewTitle;
            viewHeader = string.Format(Resources.ViewHeader, string.Empty);
        }

        protected override void OnPropertyChanging(CancelablePropertyChangingEventArgs e)
        {
            base.OnPropertyChanging(e);

            if (e.PropertyName == ExtractPropertyName(() => SelectedModule))
                PreviousModule = e.OldValue as ModuleConfig;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == ExtractPropertyName(() => SelectedModule))
            {
                ViewHeader = string.Format(Resources.ViewHeader, " - " + SelectedModule?.Name);
                //StatusService.SetStatus($"Selected {SelectedModule?.Name}");

                if (PreviousModule == null || PreviousModule == SelectedModule) return;

                var entry = ShellConfigManager.Modules.Entry(PreviousModule);
                if (entry?.IsChanged != true) return;

                MessageBoxService.Confirm(this,
                    $"Your changes in {PreviousModule.Name} will be lost. Do you want to continue?",
                    dialogCallback: rs =>
                    {
                        if (rs.Result == MessageBoxResult.Yes)
                            entry.UndoChanges();
                        else
                            SelectedModule = PreviousModule;
                        PreviousModule = null;
                    });
            }
        }

        protected override void GetToolbarItems(IToolBarSet set)
        {
            base.GetToolbarItems(set);
            set.Add(DefaultToolBarItem.Add, AddNew, "Add new Module.");
        }
    }
}