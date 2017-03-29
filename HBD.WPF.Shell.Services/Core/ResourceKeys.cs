#region

using System.Windows;

#endregion

namespace HBD.WPF.Shell.Services.Core
{
    public class ResourceKeys
    {
        public const string AddIcon = "AddIcon";
        public const string EditIcon = "EditIcon";
        public const string DeleteIcon = "DeleteIcon";
        public const string RefreshIcon = "RefreshIcon";
        public const string InfoIcon = "InfoIcon";
        public const string AlertIcon = "AlertIcon";

        public const string MenuItemInfoTemplateKey = "MenuItemInfoTemplate";
        public const string SeperatorInfoTemplateKey = "SeperatorInfoTemplate";
        public const string WorkspaceIcon = "WorkspaceIcon";
        public const string DialogIcon = "DialogIcon";
        public const string MainWindowIcon = "MainWindowIcon";
        public const string OptionIcon = "OptionIcon";
        public const string ExitIcon = "ExitIcon";
        public const string NotificationBlue = "NotificationBlue";
        public const string NotificationClearButtonStyle = "NotificationClearButtonStyle";

        public const string InfoMessageIcon = "InfoMessageIcon";
        public const string AlertMessageIcon = "AlertMessageIcon";
        public const string ConfirmMessageIcon = "ConfirmMessageIcon";

        /// <summary>
        ///     Get resource by key from Application.Current.Resources[key];
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetAppResource(string key) => Application.Current.Resources[key];
    }
}