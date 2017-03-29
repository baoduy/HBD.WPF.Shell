#region

using System.Windows;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class DialogResult : IDialogResult
    {
        public DialogResult(MessageBoxResult result) : this(result, null)
        {
        }

        public DialogResult(MessageBoxResult result, object resultValue)
        {
            Result = result;
            ResultValue = resultValue;
        }

        public MessageBoxResult Result { get; }
        public object ResultValue { get; }
    }
}