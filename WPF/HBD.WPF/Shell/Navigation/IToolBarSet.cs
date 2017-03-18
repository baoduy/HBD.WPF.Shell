#region

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#endregion

namespace HBD.WPF.Shell.Navigation
{
    public interface IToolBarSet : ICollection<UIElement>
    {
        void Add(string text, ICommand command, string icon = null, string toolTip = null);

        void Add(DefaultToolBarItem item, ICommand command, string toolTip = null);

        void AddSeparator();
    }
}