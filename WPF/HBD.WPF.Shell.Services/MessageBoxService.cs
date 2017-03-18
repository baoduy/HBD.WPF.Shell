#region

using System;
using System.ComponentModel.Composition;
using System.Windows;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;
using HBD.WPF.Shell.Services.Core;
using HBD.WPF.Shell.Views;

#endregion

namespace HBD.WPF.Shell.Services
{
    [Export(typeof(IMessageBoxService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MessageBoxService : MessageBoxServiceBase
    {
        protected override IMessageBoxOption CreateMessageBoxOption(string message, string title,
            MessageIconType iconType)
        {
            var p = new MessageBoxOption(iconType)
            {
                Title = title,
                Message = message,
                View = Container.GetInstance<IShellMessageBoxView>() as FrameworkElement
            };
            p.View.DataContext = p;

            switch (iconType)
            {
                case MessageIconType.Info:
                case MessageIconType.Alert:
                    p.Buttons = DialogButtons.Ok;
                    break;

                case MessageIconType.Confirm:
                    p.Buttons = DialogButtons.YesNo;
                    break;

                case MessageIconType.Custom:
                default:
                    throw new ArgumentOutOfRangeException(nameof(iconType), iconType, null);
            }
            return p;
        }
    }
}