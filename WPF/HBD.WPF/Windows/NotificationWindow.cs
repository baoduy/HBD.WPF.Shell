#region

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HBD.WPF.Controls;
using HBD.WPF.Core;
using HBD.WPF.EventArguments;
using HBD.WPF.Shell.Core;
using Microsoft.Expression.Interactivity.Core;

#endregion

namespace HBD.WPF.Windows
{
    public class NotificationWindow : Window, INotificationWindow
    {
        /// <summary>
        ///     Create New BackgroundProperty for the children window.
        /// </summary>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background",
            typeof(Brush), typeof(NotificationWindow), new FrameworkPropertyMetadata(SystemColors.ControlBrush));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
            typeof(NotificationPosition), typeof(NotificationWindow),
            new FrameworkPropertyMetadata(NotificationPosition.TopRight, PositionPropertyChangedCallback));

        public static readonly DependencyProperty AreaProperty = DependencyProperty.Register("Area",
            typeof(NotificationArea), typeof(NotificationWindow),
            new FrameworkPropertyMetadata(NotificationArea.MainWindow));

        public static readonly DependencyProperty NotificationsProperty = DependencyProperty.Register("Notifications",
            typeof(NotificationInfoCollection), typeof(NotificationWindow));

        private readonly NotificationInfoCollection _buffer;

        private double _currentItemHeight;

        static NotificationWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationWindow),
                new FrameworkPropertyMetadata(typeof(NotificationWindow)));
        }

        public NotificationWindow()
        {
            WindowStyle = WindowStyle.None;
            ShowActivated = false;
            ShowInTaskbar = false;
            AllowsTransparency = true;

            //ItemClosedCommand = new ActionCommand(OnItemClosed);
            ItemClickedCommand = new ActionCommand(OnItemClicked);
            ItemLoadedCommand = new ActionCommand(OnItemLoaded);
            Notifications = new NotificationInfoCollection();
            _buffer = new NotificationInfoCollection();
        }

        public new Brush Background
        {
            get { return GetValue(BackgroundProperty) as Brush; }
            set { SetValue(BackgroundProperty, value); }
        }

        public NotificationPosition Position
        {
            get { return (NotificationPosition) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public NotificationArea Area
        {
            get { return (NotificationArea) GetValue(AreaProperty); }
            set { SetValue(AreaProperty, value); }
        }

        private int MaxNotifications { get; set; } = 6;

        public INotificationInfoCollection Notifications
        {
            get { return GetValue(NotificationsProperty) as INotificationInfoCollection; }
            private set { SetValue(NotificationsProperty, value); }
        }

        //public ICommand ItemClosedCommand { get; }
        public ICommand ItemClickedCommand { get; }

        public ICommand ItemLoadedCommand { get; }

        public event EventHandler<NotificationEventArgs> ItemClick;

        public void AddNotification(INotificationInfo notification)
        {
            CalculatePosition();

            if (Notifications.Count + 1 > MaxNotifications)
                _buffer.Add(notification);
            else
                Notifications.Add(notification);

            //Show window if there're notifications
            if (Notifications.Count > 0 && !IsActive)
                Show();
        }

        private static void PositionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((NotificationWindow) d).OnPositionPropertyChanged(e);

        protected virtual void OnPositionPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = (NotificationPosition) e.NewValue;
            if (newValue == NotificationPosition.BottomLeft
                || newValue == NotificationPosition.BottomRight)
                MaxNotifications = 1;
            else MaxNotifications = 6;
        }

        private Rect GetWorkAreaRect()
        {
            if (Area == NotificationArea.PrimaryMonitor)
                return SystemParameters.WorkArea;

            var pW = Application.Current.MainWindow.RenderSize.Width;
            var pT = Application.Current.MainWindow.Top;
            var pL = Application.Current.MainWindow.Left;
            var pH = Application.Current.MainWindow.RenderSize.Height;

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                pT = 0;
                pL = 0;

                if (Application.Current.MainWindow.Left >= pW)
                    pL = MonitorExtentions.GetMainMonitor().Width;
            }

            return new Rect(pL, pT, pW, pH);
        }

        private void CalculatePosition()
        {
            var rect = GetWorkAreaRect();

            var ex = Area == NotificationArea.PrimaryMonitor ? 0 : 5;

            switch (Position)
            {
                case NotificationPosition.BottomLeft:
                    Top = rect.Height + rect.Top - _currentItemHeight;
                    Left = rect.Left + ex;
                    break;

                case NotificationPosition.BottomRight:
                    Top = rect.Height + rect.Top - _currentItemHeight;
                    Left = rect.Width + rect.Left - ex - RenderSize.Width;
                    break;

                case NotificationPosition.TopLeft:
                    Top = rect.Top;
                    Left = rect.Left + ex;
                    break;

                case NotificationPosition.TopCenter:
                    Top = rect.Top;
                    Left = rect.Left + (rect.Width - RenderSize.Width)/2;
                    break;

                case NotificationPosition.TopRight:
                default:
                    Top = rect.Top;
                    Left = rect.Width + rect.Left - ex - RenderSize.Width;
                    break;
            }
        }

        private void OnItemClicked(object obj)
        {
            var item = obj.CastAs<NotificationItem>();
            if (item == null) return;

            try
            {
                ItemClick?.Invoke(this, new NotificationEventArgs(item.NotificationInfo));
            }
            finally
            {
                item.RaiseCloseEvent();
            }
        }

        /// <summary>
        ///     calling by Item_SizeChanged
        /// </summary>
        /// <param name="obj"></param>
        private void OnItemClosed(object obj)
        {
            var item = obj.CastAs<NotificationItem>();
            if (item == null) return;
            item.SizeChanged -= Item_SizeChanged;

            RemoveNotification(item.NotificationInfo);
        }

        private void OnItemLoaded(object obj)
        {
            var item = obj.CastAs<NotificationItem>();
            if (item == null) return;
            _currentItemHeight = item.Height;
            item.SizeChanged += Item_SizeChanged;
        }

        private void Item_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var item = (NotificationItem) sender;
            if (item.Opacity <= 0.0 && e.NewSize.Height <= 0.0)
                OnItemClosed(sender);
        }

        internal void RemoveNotification(INotificationInfo notification)
        {
            //if (Notifications.Contains(notification))
            Notifications.Remove(notification);

            if (_buffer.Count > 0)
            {
                Notifications.Add(_buffer[0]);
                _buffer.RemoveAt(0);
            }

            //Close window if there's nothing to show
            if (Notifications.Count < 1)
                Hide();
        }
    }

    public enum NotificationPosition
    {
        TopLeft = 1,
        TopRight = 2,
        TopCenter = 4,
        BottomLeft = 8,
        BottomRight = 16
    }

    public enum NotificationArea
    {
        MainWindow = 1,
        PrimaryMonitor = 2
    }
}