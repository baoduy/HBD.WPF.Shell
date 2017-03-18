#region

using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Services.Core
{
    public class MessageBoxOption : DialogOption, IMessageBoxOption
    {
        public MessageBoxOption(MessageIconType iconType = MessageIconType.Info)
        {
            MessageIconType = iconType;
        }

        public MessageIconType MessageIconType { get; }

        public virtual object Icon
        {
            get
            {
                switch (MessageIconType)
                {
                    case MessageIconType.Info:
                        return ResourceKeys.GetAppResource(ResourceKeys.InfoMessageIcon);

                    case MessageIconType.Alert:
                        return ResourceKeys.GetAppResource(ResourceKeys.AlertIcon);

                    case MessageIconType.Confirm:
                        return ResourceKeys.GetAppResource(ResourceKeys.ConfirmMessageIcon);

                    case MessageIconType.Custom:
                        return ResourceKeys.GetAppResource(ResourceKeys.InfoMessageIcon);

                    default:
                        return null;
                }
            }
        }

        public string Message { get; set; }
    }
}