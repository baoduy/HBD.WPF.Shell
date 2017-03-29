#region

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

#endregion

namespace HBD.WPF.Controls
{
    public sealed class FolderDialog : IDisposable
    {
        private readonly FolderBrowserDialog _dialog;

        public FolderDialog()
        {
            _dialog = new FolderBrowserDialog();
        }

        [Browsable(true)]
        [DefaultValue("")]
        public string SelectedPath
        {
            get { return _dialog.SelectedPath; }
            set { _dialog.SelectedPath = value; }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Localizable(false)]
        public bool ShowNewFolderButton
        {
            get { return _dialog.ShowNewFolderButton; }
            set { _dialog.ShowNewFolderButton = value; }
        }

        [Browsable(true)]
        [DefaultValue(Environment.SpecialFolder.Desktop)]
        [Localizable(false)]
        public Environment.SpecialFolder RootFolder
        {
            get { return _dialog.RootFolder; }
            set { _dialog.RootFolder = value; }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get { return _dialog.Description; }
            set { _dialog.Description = value; }
        }

        public void Dispose() => _dialog.Dispose();

        public bool? ShowDialog() => ShowDialog(null);

        public bool? ShowDialog(Window owner)
        {
            DialogResult result;
            if (owner != null)
            {
                var win32Window = new NativeWindow();
                win32Window.AssignHandle(new WindowInteropHelper(owner).Handle);

                result = _dialog.ShowDialog(win32Window);
            }
            else result = _dialog.ShowDialog();

            return result == DialogResult.OK;
        }

        public void Reset() => _dialog.Reset();
    }
}