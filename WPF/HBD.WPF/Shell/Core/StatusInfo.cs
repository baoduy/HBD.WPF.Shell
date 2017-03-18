#region

using System.Windows.Media;
using HBD.Framework.Core;
using HBD.Mef.Shell.Core;

#endregion

namespace HBD.WPF.Shell.Core
{
    public class StatusInfo : Iconable, IStatusInfo
    {
        private Brush _background;
        private Brush _foreground = new SolidColorBrush(Colors.Black);
        private string _message;

        public Brush Background
        {
            get { return _background; }
            set { SetValue(ref _background, value); }
        }

        public Brush Foreground
        {
            get { return _foreground; }
            set { SetValue(ref _foreground, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }
    }
}