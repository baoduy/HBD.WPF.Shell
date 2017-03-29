#region

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

#endregion

namespace HBD.WPF.Shell.Core
{
    [DebuggerDisplay("Text = {Text}")]
    public sealed class DialogCommand
    {
        internal DialogCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public bool IsDefault { get; set; }
        public bool CloseWindowWhenClicked { get; set; } = true;
        public MessageBoxResult Result { get; set; } = MessageBoxResult.None;
    }
}