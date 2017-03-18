#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HBD.WPF;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Navigation;
using HBD.WPF.Shell.ViewModels;
using Microsoft.Expression.Interactivity.Core;
using WPF.Demo.Module.Commons;

#endregion

namespace WPF.Demo.Module
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ColorViewerViewModel : ViewModelBase, IDialogAware
    {
        private const string Brushes = "Brushes";
        private const string SystemColors = "System Colors";

        public ColorViewerViewModel()
        {
            ColorItems = new ObservableCollection<ColorItem>();
        }

        public ObservableCollection<ColorItem> ColorItems { get; }

        public void DialogActivating(object parameters, DialogCommandCollection commands)
        {
            IsActive = true;
            if (commands.Count > 0) return;
            commands.Add("Show Info", new ActionCommand(ShowInfo), false);
            commands.Add("Show View 2", new ActionCommand(ShowView2), false);
        }

        public void DialogClosing(IDialogClosingResult result)
        {
            //throw new NotImplementedException();
        }

        public void DialogClosed(IDialogResult result)
        {
            //throw new NotImplementedException();
        }

        protected override void GetToolbarItems(IToolBarSet set)
        {
            base.GetToolbarItems(set);

            var cb = new ComboBox {Width = 150};
            cb.Items.Add(string.Empty);
            cb.Items.Add(Brushes);
            cb.Items.Add(SystemColors);
            cb.SelectionChanged += (s, e) => ShowBrushes(cb.SelectedValue as string);
            set.Add(cb);
        }

        protected override void SetViewTitle(out string viewTitle, out string viewHeader)
        {
            viewTitle = "Color View";
            viewHeader = viewTitle;
        }

        private void ShowBrushes(string key)
        {
            ColorItems.Clear();

            if (string.IsNullOrWhiteSpace(key)) return;

            IEnumerable<ColorItem> list;

            if (key == Brushes)
                list =
                    typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static)
                        .Select(f => new ColorItem {Name = f.Name, Value = f.GetValue(null)});
            else
                list = typeof(SystemColors).GetProperties(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.PropertyType == typeof(Color))
                    .Select(
                        f =>
                            new ColorItem
                            {
                                Name = f.Name,
                                Value = new SolidColorBrush {Color = (Color) f.GetValue(null)}
                            });

            ColorItems.AddRange(list);
        }

        private void ShowView2()
        {
            DialogService.ShowDialog<View2>(this);
        }

        private void ShowInfo()
        {
            this.ShowBusy("The info dialog is showing...");
            MessageBoxService.Info(this, "this is info dialog for fun.", "Info AAA");
        }
    }
}