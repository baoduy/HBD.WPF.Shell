#region

using System.Windows;
using System.Windows.Input;

#endregion

namespace HBD.WPF.Core
{
    /// <summary>
    ///     This interface dedicate for Notification Control Item
    /// </summary>
    public interface INotificationItem
    {
        object DataContext { get; }
        ICommand LoadedCommand { get; set; }

        event RoutedEventHandler Close;

        void RaiseCloseEvent();
    }
}