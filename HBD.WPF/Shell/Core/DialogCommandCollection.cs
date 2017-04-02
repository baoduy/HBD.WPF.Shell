#region

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

#endregion

namespace HBD.WPF.Shell.Core
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class DialogCommandCollection : IEnumerable<DialogCommand>
    {
        internal DialogCommandCollection()
        {
            List = new List<DialogCommand>();
        }

        private IList<DialogCommand> List { get; }

        public int Count => List.Count;

        public IEnumerator<DialogCommand> GetEnumerator() => List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static DialogCommand CreateDialogCommand(string text, ICommand command, MessageBoxResult result,
                bool closeWindowWhenClicked)
            => new DialogCommand
            {
                Text = text,
                Command = command,
                CloseWindowWhenClicked = closeWindowWhenClicked,
                Result = result
            };

        public void Add(string text, ICommand command, bool closeWindowWhenClicked = true)
            => Add(CreateDialogCommand(text, command, MessageBoxResult.None, closeWindowWhenClicked));

        public void Add(string text, MessageBoxResult result, bool closeWindowWhenClicked = true)
            => Add(CreateDialogCommand(text, null, result, closeWindowWhenClicked));

        public void Insert(int index, string text, ICommand command, bool closeWindowWhenClicked = true)
            => Insert(index, CreateDialogCommand(text, command, MessageBoxResult.None, closeWindowWhenClicked));

        public void Insert(int index, string text, MessageBoxResult result, bool closeWindowWhenClicked = true)
            => Insert(index, CreateDialogCommand(text, null, result, closeWindowWhenClicked));

        internal void Add(DialogCommand command)
        {
            if (List.Contains(command)) return;
            List.Add(command);
        }

        internal void Insert(int index, DialogCommand command)
        {
            if (List.Contains(command)) return;
            List.Insert(index, command);
        }
    }
}