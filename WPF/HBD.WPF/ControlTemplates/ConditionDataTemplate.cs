#region

using System;
using System.Windows;

#endregion

namespace HBD.WPF.ControlTemplates
{
    public class ConditionDataTemplate : DependencyObject
    {
        public static readonly DependencyProperty DataTypeProperty = DependencyProperty.Register("DataType",
            typeof(Type), typeof(ConditionDataTemplate));

        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding",
            typeof(object), typeof(ConditionDataTemplate));

        public static readonly DependencyProperty DataTemplateProperty = DependencyProperty.Register("DataTemplate",
            typeof(DataTemplate), typeof(ConditionDataTemplate));

        public Type DataType
        {
            get { return GetValue(DataTypeProperty) as Type; }
            set { SetValue(DataTypeProperty, value); }
        }

        public object Binding
        {
            get { return GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        public DataTemplate DataTemplate
        {
            get { return (DataTemplate) GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        public bool IsValid => Binding?.GetType() == DataType;
    }
}