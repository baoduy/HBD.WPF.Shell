#region

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Controls
{
    [TemplatePart(Name = NamePartCloseButton, Type = typeof(Button))]
    public class NotificationItem : Control, INotificationItem
    {
        public const string NamePartCloseButton = "PART_CloseButton";

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand",
            typeof(ICommand), typeof(NotificationItem));

        public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.Register("LoadedCommand",
            typeof(ICommand), typeof(NotificationItem));

        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register("IconWidth",
            typeof(double), typeof(NotificationItem), new FrameworkPropertyMetadata(20d));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius), typeof(NotificationItem),
            new FrameworkPropertyMetadata(new CornerRadius(),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(NotificationItem));

        private Button _partCloseButton;

        static NotificationItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationItem),
                new FrameworkPropertyMetadata(typeof(NotificationItem)));
        }

        public NotificationItem()
        {
            Loaded += (s, e) => LoadedCommand?.Execute(this);
        }

        public INotificationInfo NotificationInfo
        {
            get { return DataContext as INotificationInfo; }
            set { DataContext = value; }
        }

        public double IconWidth
        {
            get { return (double) GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public ICommand ClickCommand
        {
            get { return GetValue(ClickCommandProperty) as ICommand; }
            set { SetValue(ClickCommandProperty, value); }
        }

        public event RoutedEventHandler Close
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
        }

        public void RaiseCloseEvent()
        {
            var newEvent = new RoutedEventArgs(CloseEvent, this);
            RaiseEvent(newEvent);
        }

        public ICommand LoadedCommand
        {
            get { return GetValue(LoadedCommandProperty) as ICommand; }
            set { SetValue(LoadedCommandProperty, value); }
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _partCloseButton = GetTemplateChild(NamePartCloseButton) as Button;

            if (_partCloseButton != null)
                _partCloseButton.Click += (s, e) => OnClose();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            e.Handled = true;

            ClickCommand?.Execute(this);
        }
    }
}