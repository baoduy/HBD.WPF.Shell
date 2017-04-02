#region

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using HBD.WPF.Core;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.Shell.UI.Controls
{
    /// <summary>
    ///     Interaction logic for GroupNotificationItem.xaml
    /// </summary>
    public partial class GroupNotificationItem : INotificationItem
    {
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand",
            typeof(ICommand), typeof(GroupNotificationItem));

        public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.Register("LoadedCommand",
            typeof(ICommand), typeof(GroupNotificationItem));

        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(GroupNotificationItem));

        public GroupNotificationItem()
        {
            InitializeComponent();

            CloseCommand = new ActionCommand(OnClose);
            Loaded += (s, e) => LoadedCommand?.Execute(this);
        }

        public ICommand CloseCommand
        {
            get { return GetValue(CloseCommandProperty) as ICommand; }
            set { SetValue(CloseCommandProperty, value); }
        }

        public event RoutedEventHandler Close
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
        }

        public ICommand LoadedCommand
        {
            get { return GetValue(LoadedCommandProperty) as ICommand; }
            set { SetValue(LoadedCommandProperty, value); }
        }

        public void RaiseCloseEvent()
        {
            var newEvent = new RoutedEventArgs(CloseEvent, this);
            RaiseEvent(newEvent);
        }

        protected virtual void OnClose()
        {
            Height = ActualHeight;

            if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
            {
                var peer = UIElementAutomationPeer.CreatePeerForElement(this);
                peer?.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
            }

            RaiseCloseEvent();
        }
    }
}