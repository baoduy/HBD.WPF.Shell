#region

using System.Windows.Controls.Primitives;
using System.Windows.Data;

#endregion

namespace HBD.WPF.Controls
{
    public class ToolBar : System.Windows.Controls.ToolBar
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var overflowPanel = GetTemplateChild("OverflowButton") as ToggleButton;
            if (overflowPanel != null)
                BindingOperations.SetBinding(overflowPanel, BackgroundProperty,
                    new Binding("Background") {Source = this});
        }
    }
}