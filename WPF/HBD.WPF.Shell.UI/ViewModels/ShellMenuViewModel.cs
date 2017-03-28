#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using HBD.Framework;
using HBD.Mef.Logging;
using HBD.Mef.Shell.Navigation;
using HBD.Mef.Shell.Services;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.UI.Common;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.Shell.UI.ViewModels
{
    [Export(typeof(IShellMenuViewModel))]
    [Export(typeof(IShellMenuService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShellMenuViewModel : ViewModelBase, IShellMenuViewModel
    {
        private ICommand _mainMenuCommand;

        public ShellMenuViewModel()
        {
            InternalCollection = new LeftRightMenuCollection();
            MainMenuCommand = new ActionCommand(MainMenuAction);
        }

        private LeftRightMenuCollection InternalCollection { get; }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public INavigationParameterExecuter NavigationExecuter { protected get; set; }

        public ICommand MainMenuCommand
        {
            get { return _mainMenuCommand; }
            set { SetValue(ref _mainMenuCommand, value); }
        }

        public ObservableCollection<IMenuInfo> LeftMenuItems => InternalCollection.LeftMenuItems;
        public ObservableCollection<IMenuInfo> RightMenuItems => InternalCollection.RightMenuItems;

        protected virtual void MainMenuAction(object obj)
        {
            var nv = obj as INavigationInfo;

            if (nv == null || nv.NavigationParameters.Count <= 0) return;

            try
            {
                //Validate Roles
                //if (AuthenticationService != null && !AuthenticationService.IsValid(nv.PermissionValidation))
                //{
                //    MessageBoxService.Alert(this, "You are not authorized to access this action.");
                //    return;
                //}
                nv.NavigationParameters.ForEach(ExecuteNavigation);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                MessageBoxService.Alert(this, ex.Message);
            }
        }

        private void ExecuteNavigation(INavigationParameter parameter)
            => NavigationExecuter.Execute(parameter, DefaultNavigationCallback);

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
            => viewHeader = viewTitle = string.Empty;

        #region IMenuInfoCollection

        public int Count => InternalCollection.Count;

        public bool IsReadOnly => false;

        public IMenuInfo this[int index] => InternalCollection[index];

        public void Add(IMenuInfo item) => InternalCollection.Add(item);

        public void Clear() => InternalCollection.Clear();

        public bool Contains(IMenuInfo item) => InternalCollection.Contains(item);

        public void CopyTo(IMenuInfo[] array, int arrayIndex) => InternalCollection.CopyTo(array, arrayIndex);

        public IEnumerator<IMenuInfo> GetEnumerator() => InternalCollection.GetEnumerator();

        public bool Remove(IMenuInfo item) => InternalCollection.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //public int IndexOf(IMenuInfo item) => InternalCollection.IndexOf(item);

        //public void Insert(int index, IMenuInfo item) => InternalCollection.Insert(index, item);

        //public void RemoveAt(int index) => InternalCollection.RemoveAt(index);

        #endregion IMenuInfoCollection
    }
}