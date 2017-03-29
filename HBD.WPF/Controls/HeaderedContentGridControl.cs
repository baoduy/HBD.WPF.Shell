#region

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace HBD.WPF.Controls
{
    public class HeaderedContentGridControl : ContentControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object),
            typeof(HeaderedContentGridControl));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", typeof(DataTemplate), typeof(HeaderedContentGridControl));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector),
                typeof(HeaderedContentGridControl));

        static HeaderedContentGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedContentGridControl),
                new FrameworkPropertyMetadata(typeof(HeaderedContentGridControl)));
        }

        [Bindable(true)]
        [Category("Content")]
        [Localizability(LocalizationCategory.Label)]
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        [Bindable(true)]
        [Category("Content")]
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate) GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        [Bindable(true)]
        [Category("Content")]
        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }
    }
}