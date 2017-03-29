#region

using System;
using System.Collections.Generic;
using System.Windows;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF
{
    public static class DialogButtonsExtensions
    {
        public static IEnumerable<DialogCommand> GetCommands(this DialogButtons dialogButtons)
        {
            var okCommand = new DialogCommand {Text = "_OK", Result = MessageBoxResult.OK};
            var cancelCommand = new DialogCommand {Text = "_Cancel", Result = MessageBoxResult.Cancel, IsDefault = true};
            var noCommand = new DialogCommand {Text = "_No", Result = MessageBoxResult.No};
            var yesCommand = new DialogCommand {Text = "_Yes", Result = MessageBoxResult.Yes};

            switch (dialogButtons)
            {
                case DialogButtons.Ok:
                    okCommand.IsDefault = true;
                    yield return okCommand;
                    break;

                case DialogButtons.OkCancel:
                    yield return okCommand;
                    yield return cancelCommand;
                    break;

                case DialogButtons.YesNoCancel:
                    yield return yesCommand;
                    yield return noCommand;
                    yield return cancelCommand;
                    break;

                case DialogButtons.YesNo:
                    noCommand.IsDefault = true;
                    yield return yesCommand;
                    yield return noCommand;
                    break;

                case DialogButtons.None:
                    break;

                case DialogButtons.Custom:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogButtons), dialogButtons, null);
            }
        }
    }
}