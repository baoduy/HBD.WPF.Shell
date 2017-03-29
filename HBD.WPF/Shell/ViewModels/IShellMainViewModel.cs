#region

using System.Windows;

#endregion

namespace HBD.WPF.Shell.ViewModels
{
    public interface IShellMainViewModel : IViewModel
    {
        //ObservableCollection<FrameworkElement> ActivatedObjects { get; }
        FrameworkElement SelectedElement { get; set; }
    }
}