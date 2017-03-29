#region

using System.Windows;
using System.Windows.Media.Animation;

#endregion

namespace HBD.WPF.AttacheControls
{
    public class PropertyChangedAnimation : DependencyObject
    {
        public static DependencyProperty BindingProperty =
            DependencyProperty.RegisterAttached("Binding", typeof(object), typeof(PropertyChangedAnimation),
                new PropertyMetadata(BindingChanged));

        public static DependencyProperty StoryboardProperty =
            DependencyProperty.RegisterAttached("Storyboard", typeof(Storyboard), typeof(PropertyChangedAnimation));

        public static object GetBinding(DependencyObject e) => e.GetValue(BindingProperty);

        public static void SetBinding(DependencyObject e, object value) => e.SetValue(BindingProperty, value);

        public static Storyboard GetStoryboard(DependencyObject e) => e.GetValue(StoryboardProperty) as Storyboard;

        public static void SetStoryboard(DependencyObject e, Storyboard value) => e.SetValue(StoryboardProperty, value);

        private static void BindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var storyboard = GetStoryboard(d);
            if (storyboard == null) return;

            Storyboard.SetTarget(d, storyboard);
            ((FrameworkElement) d).BeginStoryboard(storyboard);
        }
    }
}