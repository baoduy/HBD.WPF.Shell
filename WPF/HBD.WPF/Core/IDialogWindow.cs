#region

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Core
{
    public interface IDialogWindow
    {
        ObservableCollection<DialogCommand> DialogCommands { get; }
        MessageBoxResult MessageBoxResult { get; set; }

        string Title { get; set; }

        //Brush TitleBackground { get; set; }
        //Brush TitleForeground { get; set; }
        //Visibility TitleVisibility { get; set; }
        //double TitleHeight { get; set; }
        FrameworkElement Owner { get; set; }

        object Content { get; set; }
        bool IsShown { get; }

        event EventHandler Shown;

        event EventHandler Closed;

        event CancelEventHandler Closing;

        void Close();

        bool? ShowDialog();

        void Show();
    }
}