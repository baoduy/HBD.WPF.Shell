using System.Windows;
using System.Windows.Controls;

namespace LoadingIndicators.WPF
{
    /// <summary>
    /// A control featuring a range of loading indicating animations.
    /// </summary>
    [TemplatePart(Name = "Border", Type = typeof(Border))]
    public class LoadingIndicator : Control
    {
        /// <summary>
        /// Identifies the <see cref="LoadingIndicators.WPF.LoadingIndicator.SpeedRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpeedRatioProperty =
            DependencyProperty.Register("SpeedRatio", typeof(double), typeof(LoadingIndicator), new PropertyMetadata(1d, (o, e) => {
                var li = (LoadingIndicator) o;

                if (li.PartBorder == null || li.IsActive == false) {
                    return;
                }

                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(li.PartBorder))
                {
                    if (@group.Name != "ActiveStates") continue;
                    foreach (VisualState state in @group.States) {
                        if (state.Name == "Active") {
                            state.Storyboard.SetSpeedRatio(li.PartBorder, (double) e.NewValue);
                        }
                    }
                }
            }));

        /// <summary>
        /// Identifies the <see cref="LoadingIndicators.WPF.LoadingIndicator.IsActive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(true, (o, e) => {
                LoadingIndicator li = (LoadingIndicator) o;

                if (li.PartBorder == null) {
                    return;
                }

                if ((bool) e.NewValue == false) {
                    VisualStateManager.GoToElementState(li.PartBorder, "Inactive", false);
                    li.PartBorder.Visibility = Visibility.Collapsed;
                } else {
                    VisualStateManager.GoToElementState(li.PartBorder, "Active", false);
                    li.PartBorder.Visibility = Visibility.Visible;

                    foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(li.PartBorder))
                    {
                        if (@group.Name != "ActiveStates") continue;
                        foreach (VisualState state in @group.States) {
                            if (state.Name == "Active") {
                                state.Storyboard.SetSpeedRatio(li.PartBorder, li.SpeedRatio);
                            }
                        }
                    }
                }
            }));

        // Variables
        protected Border PartBorder;

        /// <summary>
        /// Get/set the speed ratio of the animation.
        /// </summary>
        public double SpeedRatio
        {
            get { return (double) GetValue(SpeedRatioProperty); }
            set { SetValue(SpeedRatioProperty, value); }
        }

        /// <summary>
        /// Get/set whether the loading indicator is active.
        /// </summary>
        public bool IsActive
        {
            get { return (bool) GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PartBorder = (Border) GetTemplateChild("PART_Border");

            if (PartBorder == null) return;
            VisualStateManager.GoToElementState(PartBorder, (this.IsActive ? "Active" : "Inactive"), false);
            foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(PartBorder))
            {
                if (@group.Name != "ActiveStates") continue;
                foreach (VisualState state in @group.States) {
                    if (state.Name == "Active") {
                        state.Storyboard.SetSpeedRatio(PartBorder, this.SpeedRatio);
                    }
                }
            }

            PartBorder.Visibility = (this.IsActive ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
