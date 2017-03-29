#region

using System.Windows;
using HBD.WPF.Core;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class DialogOption : IDialogOption
    {
        private DialogCommandCollection _customCommands;

        public FrameworkElement View { get; set; }
        public string Title { get; set; }
        public DialogButtons Buttons { get; set; } = DialogButtons.Ok;

        public DialogCommandCollection CustomCommands
        {
            get
            {
                if (_customCommands == null)
                    Initialize();
                return _customCommands;
            }
        }

        /// <summary>
        ///     Auto: Show Model and auto switch to Dialog If Model is showing.
        /// </summary>
        public DialogType DialogType { get; set; } = DialogType.Auto;

        public object Parameters { get; set; }

        protected virtual void Initialize()
        {
            _customCommands = new DialogCommandCollection();

            foreach (var cmd in Buttons.GetCommands())
                CustomCommands.Add(cmd);
        }
    }

    public enum DialogType
    {
        Auto,
        Dialog,
        Model
    }
}