#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HBD.WPF.AttacheControls;
using HBD.WPF.Commands;
using HBD.WPF.Core;
using HBD.WPF.Shell.Core;

#endregion

namespace HBD.WPF.Windows
{
    public class DialogWindow : Window, IDialogWindow
    {
        public static readonly DependencyProperty MessageBoxResultProperty =
            DependencyProperty.Register("MessageBoxResult", typeof(MessageBoxResult), typeof(DialogWindow),
                new FrameworkPropertyMetadata(MessageBoxResult.None));

        public static readonly DependencyProperty DialogCommandsProperty = DependencyProperty.Register(
            "DialogCommands", typeof(ObservableCollection<DialogCommand>), typeof(DialogWindow));

        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register("IsShown", typeof(bool),
            typeof(DialogWindow), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty CommandButtonActionProperty =
            DependencyProperty.Register("CommandButtonAction", typeof(RelayCommand), typeof(DialogWindow));

        private bool _isInitialised;

        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow),
                new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }

        public DialogWindow()
        {
            WindowCustomizer.SetCanMinimize(this, false);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            DialogCommands = new ObservableCollection<DialogCommand>();
            CommandButtonAction = new RelayCommand(CommandButtonExecution, CanExecuteCommandButton);
        }

        public RelayCommand CommandButtonAction
        {
            get { return GetValue(CommandButtonActionProperty) as RelayCommand; }
            private set { SetValue(CommandButtonActionProperty, value); }
        }

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

        public new FrameworkElement Owner
        {
            get { return base.Owner; }
            set { base.Owner = (Window) value; }
        }

        public bool IsShown
        {
            get { return (bool) GetValue(IsShownProperty); }
            set { SetValue(IsShownProperty, value); }
        }

        public event EventHandler Shown;

        //public new void ShowDialog() => base.ShowDialog();
        protected virtual void OnShown(EventArgs e) => Shown?.Invoke(this, e);

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (_isInitialised) return;

            _isInitialised = true;
            SizeToContent = SizeToContent.Manual;
            OnShown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _isInitialised = false;
            SizeToContent = SizeToContent.WidthAndHeight;
            base.OnClosed(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Close();
            }
            base.OnKeyUp(e);
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

        //private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}