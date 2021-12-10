using Ninject;
using Avalonia.Controls;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel([Named("DevicesList")] UserControl devicesList)
        {
            DevicesList = devicesList;
        }

        public UserControl DevicesList { get; }
    }
}
