#region

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace HBD.WPF.Shell.Services
{
    /// <summary>
    ///     This service will helps shell to manager the setting:
    ///     - The current window state.
    ///     - With Width, Height of Main window
    ///     - The position of tabs.
    ///     - Save and load status when application save or re-open.
    /// </summary>
    public interface IShellOptionService : INotifyPropertyChanged
    {
        /// <summary>
        ///     The WindowState
        /// </summary>
        WindowState WindowState { get; set; }

        WindowStyle WindowStyle { get; set; }

        /// <summary>
        ///     The name of current theme.
        /// </summary>
        string Theme { get; set; }

        /// <summary>
        ///     The TabStripPlacement of MainView
        /// </summary>
        Dock TabStripPlacement { get; set; }

        double Width { get; set; }
        double Height { get; set; }
        double Left { get; set; }
        double Top { get; set; }
        bool Topmost { get; set; }

        void LoadSetting();

        void SaveSetting();
    }
}