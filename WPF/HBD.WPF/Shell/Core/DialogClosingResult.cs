#region

using System.Windows;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class DialogClosingResult : IDialogClosingResult
    {
        public DialogClosingResult(MessageBoxResult result) : this(result, null)
        {
        }

        public DialogClosingResult(MessageBoxResult result, object resultValue)
        {
            Result = result;
            ResultValue = resultValue;
        }

        public bool Cancel { get; set; }
        public MessageBoxResult Result { get; set; }
        public object ResultValue { get; }
    }
}