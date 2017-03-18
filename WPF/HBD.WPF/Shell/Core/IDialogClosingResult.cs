namespace HBD.WPF.Shell.Core
{
    public interface IDialogClosingResult : IDialogResult
    {
        bool Cancel { get; set; }
    }
}