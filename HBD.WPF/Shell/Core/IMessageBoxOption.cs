namespace HBD.WPF.Shell.Core
{
    public interface IMessageBoxOption : IDialogOption
    {
        MessageIconType MessageIconType { get; }
        object Icon { get; }
        string Message { get; set; }
    }
}