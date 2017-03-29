#region

using System.Linq;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace HBD.WPF.ControlTemplates
{
    public class ConditionDataTemplateSelector : DataTemplateSelector
    {
        public ConditionDataTemplateSelector()
        {
            Items = new TemplateCollection();
        }

        public TemplateCollection Items { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var c = container as FrameworkElement;

            if (Items == null || Items.Count <= 0 || c == null)
                return base.SelectTemplate(item, container);

            var t = Items.FirstOrDefault(i => i.DataType == c.DataContext.GetType());
            if (t == null)
                return base.SelectTemplate(item, container);

            return t.DataTemplate;
        }
    }
}