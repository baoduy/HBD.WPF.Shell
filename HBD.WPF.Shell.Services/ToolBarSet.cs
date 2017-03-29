#region

using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HBD.WPF.Controls;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.Services.Core;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(IToolBarSet))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ToolBarSet : ObservableCollection<UIElement>, IToolBarSet
    {
        public void Add(string text, ICommand command, string icon = null, string toolTip = null)
        {
            var bt = CreateItem(text, command, icon, toolTip);
            Add(bt);
        }

        public virtual void Add(DefaultToolBarItem item, ICommand command, string toolTip = null)
        {
            var bt = CreateItem(item.ToString(), command, null, toolTip);
            bt.Icon = GetDefaultIcon(item);
            Add(bt);
        }

        public void AddSeparator() => Add(new Separator());

        private static ToolBarItem CreateItem(string text, ICommand command, string icon = null, string toolTip = null)
        {
            var bt = new ToolBarItem
            {
                Text = text,
                Command = command,
                ToolTip = toolTip,
                Icon = icon,
                IsEnabled = command != null
            };

            return bt;
        }

        protected virtual object GetDefaultIcon(DefaultToolBarItem item)
        {
            switch (item)
            {
                case DefaultToolBarItem.Add:
                    return ResourceKeys.GetAppResource(ResourceKeys.AddIcon);

                case DefaultToolBarItem.Edit:
                    return ResourceKeys.GetAppResource(ResourceKeys.EditIcon);

                case DefaultToolBarItem.Remove:
                    return ResourceKeys.GetAppResource(ResourceKeys.DeleteIcon);

                case DefaultToolBarItem.Refresh:
                    return ResourceKeys.GetAppResource(ResourceKeys.RefreshIcon);

                default:
                    return null;
            }
        }
    }
}