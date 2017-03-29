#region

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using HBD.WPF.Commands;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Controls
{
    [ContentProperty("Content")]
    [TemplatePart(Name = NamePartModelRoot, Type = typeof(Canvas))]
    [TemplatePart(Name = NamePartWindowRoot, Type = typeof(Border))]
    [TemplatePart(Name = NamePartHeader, Type = typeof(Border))]
    [TemplatePart(Name = NamePartResize, Type = typeof(Border))]
    [TemplatePart(Name = NamePartCloseButton, Type = typeof(Button))]
    public class ModelWindow : Control, IDialogWindow
    {
        public const string NamePartModelRoot = "PART_ModelRoot";
        public const string NamePartWindowRoot = "PART_WindowRoot";
        public const string NamePartHeader = "PART_Header";
        public const string NamePartResize = "PART_Resize";
        public const string NamePartCloseButton = "PART_CloseButton";
        private object _currentDataContext;

        private bool _isDrag;
        private bool _isResize;
        private Point _lastPoint = default(Point);
        private Button _partCloseButton;
        private Border _partHeader;
        private Canvas _partModelRoot;
        private Border _partResize;
        private Border _partWindowRoot;

        static ModelWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModelWindow),
                new FrameworkPropertyMetadata(typeof(ModelWindow)));
        }

        public ModelWindow()
        {
            DialogCommands = new ObservableCollection<DialogCommand>();
            CommandButtonAction = new RelayCommand(CommandButtonExecution, CanExecuteCommandButton);
        }

        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public Brush TitleBackground
        {
            get { return GetValue(TitleBackgroundProperty) as Brush; }
            set { SetValue(TitleBackgroundProperty, value); }
        }

        public Brush TitleForeground
        {
            get { return GetValue(TitleForegroundProperty) as Brush; }
            set { SetValue(TitleForegroundProperty, value); }
        }

        public Visibility TitleVisibility
        {
            get { return (Visibility) GetValue(TitleVisibilityProperty); }
            set
            {
                SetValue(TitleVisibilityProperty, value);
                if (value != Visibility.Visible)
                    TitleHeight = 0;
            }
        }

        public double TitleHeight
        {
            get { return (double) GetValue(TitleHeightProperty); }
            set { SetValue(TitleHeightProperty, value); }
        }

        public Visibility CloseButtonVisibility
        {
            get { return (Visibility) GetValue(CloseButtonVisibilityProperty); }
            set { SetValue(CloseButtonVisibilityProperty, value); }
        }

        public Brush WindowBackground
        {
            get { return GetValue(WindowBackgroundProperty) as Brush; }
            set { SetValue(WindowBackgroundProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return GetValue(ContentTemplateProperty) as DataTemplate; }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public new Visibility Visibility => base.Visibility;

        public RelayCommand CommandButtonAction
        {
            get { return GetValue(CommandButtonActionProperty) as RelayCommand; }
            private set { SetValue(CommandButtonActionProperty, value); }
        }

        public FrameworkElement Owner { get; set; }

        public ObservableCollection<DialogCommand> DialogCommands
        {
            get { return GetValue(DialogCommandsProperty) as ObservableCollection<DialogCommand>; }
            set { SetValue(DialogCommandsProperty, value); }
        }

        public MessageBoxResult MessageBoxResult
        {
            get { return (MessageBoxResult) GetValue(MessageBoxResultProperty); }
            set { SetValue(MessageBoxResultProperty, value); }
        }

        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        ///     Indicate the window is shown or not.
        /// </summary>
        public bool IsShown
        {
            get { return (bool) GetValue(IsShownProperty); }
            set { SetValue(IsShownProperty, value); }
        }

        public virtual void Show() => OnShown(EventArgs.Empty);

        public virtual bool? ShowDialog()
        {
            Show();
            return null;
        }

        public virtual void Close() => OnClosing();

        public event EventHandler Shown;

        public event EventHandler Closed;

        public event CancelEventHandler Closing;

        protected virtual void OnIconChanged(object newIcon)
            => OnPropertyChanged(new DependencyPropertyChangedEventArgs(IconProperty, Icon, newIcon));

        protected virtual void OnShown(EventArgs e)
        {
            IsShown = true;
            Shown?.Invoke(this, e);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, e);

            _currentDataContext = null;
            RemoveAllHandledEvents();
        }

        protected virtual void OnClosing()
        {
            if (Visibility != Visibility.Visible) return;
            var e = new CancelEventArgs();

            Closing?.Invoke(this, e);

            if (e.Cancel) return;
            IsShown = false;
            OnClosed(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _partModelRoot = GetTemplateChild(NamePartModelRoot) as Canvas;
            _partWindowRoot = GetTemplateChild(NamePartWindowRoot) as Border;
            _partHeader = GetTemplateChild(NamePartHeader) as Border;
            _partResize = GetTemplateChild(NamePartResize) as Border;
            _partCloseButton = GetTemplateChild(NamePartCloseButton) as Button;

            if (_partModelRoot != null)
                _partModelRoot.SizeChanged += _partModelRoot_SizeChanged;

            if (_partWindowRoot != null)
                _partWindowRoot.SizeChanged += _partModelRoot_SizeChanged;

            if (_partCloseButton != null)
                _partCloseButton.Click += _partCloseButton_Click;

            if (_partHeader != null)
            {
                _partHeader.MouseLeftButtonDown += Header_MouseLeftButtonDown;
                _partHeader.MouseLeftButtonUp += Header_MouseLeftButtonUp;
                _partHeader.MouseMove += Header_MouseMove;
                _partHeader.LostMouseCapture += Header_LostMouseCapture;
            }

            if (_partResize != null)
            {
                _partResize.MouseLeftButtonDown += Border_MouseLeftButtonDown;
                _partResize.MouseLeftButtonUp += Border_MouseLeftButtonUp;
                _partResize.MouseMove += Border_MouseMove;
                _partResize.LostMouseCapture += Border_MouseLeave;
            }
        }

        private void _partCloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void _partModelRoot_SizeChanged(object sender, SizeChangedEventArgs e) => MoveToCenter();

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (Parent == null) return;

            if (e.Property == IsShownProperty)
            {
                base.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Hidden;
                MoveToCenter();
            }

            if (e.Property == ContentProperty)
                SetWindowSizeWithViewSize();

            if (e.Property == ContentProperty)
                SetWindowSizeWithViewSize();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            SetWindowSizeWithViewSize();

            base.OnRender(drawingContext);

            var p = Parent as FrameworkElement;
            if (p == null) return;

            _currentDataContext = p.DataContext;
            p.DataContextChanged += Parent_DataContextChanged;
        }

        private void Parent_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == _currentDataContext && IsShown)
                base.Visibility = Visibility.Visible;
            else base.Visibility = Visibility.Collapsed;
        }

        private void CommandButtonExecution(object parameter)
        {
            if (!(parameter is Guid)) return;

            var cmd = DialogCommands.FirstOrDefault(c => c.Id == (Guid) parameter);
            if (cmd == null) return;

            MessageBoxResult = cmd.Result;
            cmd.Command?.Execute(cmd);

            if (cmd.CloseWindowWhenClicked)
                Close();
        }

        private bool CanExecuteCommandButton(object parameter)
        {
            if (!(parameter is Guid)) return true;

            var cmd = DialogCommands.FirstOrDefault(c => c.Id == (Guid) parameter);
            if (cmd == null) return false;

            return cmd.Command == null || cmd.Command.CanExecute(parameter);
        }

        private void SetWindowSizeWithViewSize()
        {
            var view = Content as FrameworkElement;
            if (view == null || _partWindowRoot == null) return;

            //Fix the Width and Height of Window. So that it wont be resize when child's size changed.
            var minH = Math.Min(view.MinHeight, _partWindowRoot.MaxHeight);
            var minW = Math.Min(view.MinWidth, _partWindowRoot.MaxWidth);
            _partWindowRoot.MinHeight = Math.Max(minH, _partWindowRoot.MinHeight);
            _partWindowRoot.MinWidth = Math.Max(minW, _partWindowRoot.MinWidth);
            _partWindowRoot.Height = Math.Max(view.ActualHeight, view.Height);
            _partWindowRoot.Width = Math.Max(view.ActualWidth, view.Width);

            MoveToCenter();
        }

        private void MoveToCenter()
        {
            if (!IsShown || _isResize) return;
            if (_partModelRoot == null || _partWindowRoot == null) return;

            var left = (_partModelRoot.ActualWidth - _partWindowRoot.ActualWidth)/2;
            var top = (_partModelRoot.ActualHeight - _partWindowRoot.ActualHeight)/2;

            Canvas.SetLeft(_partWindowRoot, left);
            Canvas.SetTop(_partWindowRoot, top);
        }

        private void RemoveAllHandledEvents()
        {
            if (Shown != null)
                foreach (var @delegate in Shown.GetInvocationList())
                {
                    var h = (EventHandler) @delegate;
                    Shown -= h;
                }

            if (Closing != null)
                foreach (var @delegate in Closing.GetInvocationList())
                {
                    var h = (CancelEventHandler) @delegate;
                    Closing -= h;
                }

            if (Closed != null)
                foreach (var @delegate in Closed.GetInvocationList())
                {
                    var h = (EventHandler) @delegate;
                    Closed -= h;
                }

            var p = Parent as FrameworkElement;
            if (p != null)
                p.DataContextChanged -= Parent_DataContextChanged;
        }

        #region Properties

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object),
            typeof(ModelWindow),
            new FrameworkPropertyMetadata(_OnIconChanged));

        private static void _OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var w = d as ModelWindow;
            w?.OnIconChanged(e.NewValue);
        }

        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register("IsShown", typeof(bool),
            typeof(ModelWindow), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(ModelWindow),
                new FrameworkPropertyMetadata(Brushes.White));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(ModelWindow),
                new FrameworkPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string),
            typeof(ModelWindow), new FrameworkPropertyMetadata("Model Window"));

        public static readonly DependencyProperty TitleVisibilityProperty =
            DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(ModelWindow),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.Register("TitleHeight",
            typeof(double), typeof(ModelWindow), new FrameworkPropertyMetadata(25d));

        public static readonly DependencyProperty WindowBackgroundProperty =
            DependencyProperty.Register("WindowBackground", typeof(Brush), typeof(ModelWindow),
                new FrameworkPropertyMetadata(SystemColors.ControlBrush));

        public static readonly DependencyProperty CloseButtonVisibilityProperty =
            DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(ModelWindow),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty DialogCommandsProperty = DependencyProperty.Register(
            "DialogCommands", typeof(ObservableCollection<DialogCommand>), typeof(ModelWindow));

        public static readonly DependencyProperty CommandButtonActionProperty =
            DependencyProperty.Register("CommandButtonAction", typeof(RelayCommand), typeof(ModelWindow));

        public static readonly DependencyProperty MessageBoxResultProperty =
            DependencyProperty.Register("MessageBoxResult", typeof(MessageBoxResult), typeof(ModelWindow),
                new FrameworkPropertyMetadata(MessageBoxResult.None));

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(ModelWindow));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object), typeof(ModelWindow));

        //Override default value of Parent Properties
        public new static readonly DependencyProperty BorderThicknessProperty =
            Control.BorderThicknessProperty.AddOwner(typeof(ModelWindow),
                new FrameworkPropertyMetadata(new Thickness(2)));

        public new static readonly DependencyProperty BackgroundProperty =
            Control.BackgroundProperty.AddOwner(typeof(ModelWindow),
                new FrameworkPropertyMetadata(new SolidColorBrush {Color = Colors.Gray, Opacity = 0.7}));

        #endregion

        #region Events

        #endregion

        #region Moving Window

        private void DragCaptureElement(IInputElement element)
        {
            _isDrag = true;
            element?.CaptureMouse();
        }

        private void ReleaseDragCaptureElement(IInputElement element)
        {
            element?.ReleaseMouseCapture();
            _isDrag = false;
            _lastPoint = default(Point);
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastPoint = e.GetPosition(this);
            DragCaptureElement(sender as FrameworkElement);
        }

        private void Header_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => ReleaseDragCaptureElement(sender as FrameworkElement);

        private void Header_LostMouseCapture(object sender, MouseEventArgs e)
            => ReleaseDragCaptureElement(sender as FrameworkElement);

        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrag || e.LeftButton == MouseButtonState.Released)
            {
                ReleaseDragCaptureElement(sender as FrameworkElement);
                return;
            }
            var currentPoint = e.GetPosition(this);
            var currentLeft = Canvas.GetLeft(_partWindowRoot);
            var currentTop = Canvas.GetTop(_partWindowRoot);
            var left = currentLeft + (currentPoint.X - _lastPoint.X);
            var top = currentTop + (currentPoint.Y - _lastPoint.Y);

            if (left < 0) left = 0;
            if (left > _partWindowRoot.MaxWidth - _partWindowRoot.ActualWidth)
                left = _partWindowRoot.MaxWidth - _partWindowRoot.ActualWidth;

            if (top < 0) top = 0;
            if (top > _partWindowRoot.MaxHeight - _partWindowRoot.ActualHeight)
                top = _partWindowRoot.MaxHeight - _partWindowRoot.ActualHeight;

            Canvas.SetLeft(_partWindowRoot, left);
            Canvas.SetTop(_partWindowRoot, top);

            _lastPoint = currentPoint;
        }

        #endregion

        #region Resize Window

        private void ResizeCaptureElement(IInputElement element)
        {
            _isResize = true;
            element?.CaptureMouse();
        }

        private void ReleaseResizeCaptureElement(IInputElement element)
        {
            element?.ReleaseMouseCapture();
            _isResize = false;
            _lastPoint = default(Point);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastPoint = e.GetPosition(this);
            ResizeCaptureElement(sender as FrameworkElement);
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isResize || e.LeftButton == MouseButtonState.Released)
            {
                ReleaseDragCaptureElement(sender as FrameworkElement);
                return;
            }
            var currentPoint = e.GetPosition(this);
            var x = currentPoint.X - _lastPoint.X;
            var y = currentPoint.Y - _lastPoint.Y;

            _partWindowRoot.Height = _partWindowRoot.ActualHeight + y;
            _partWindowRoot.Width = _partWindowRoot.ActualWidth + x;

            _lastPoint = currentPoint;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => ReleaseResizeCaptureElement(sender as FrameworkElement);

        private void Border_MouseLeave(object sender, MouseEventArgs e)
            => ReleaseResizeCaptureElement(sender as FrameworkElement);

        #endregion
    }
}