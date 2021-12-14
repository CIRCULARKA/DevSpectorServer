using Ninject;
using Avalonia.Controls;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(
            [Named("DevicesList")] UserControl devicesList,
            [Named("DeviceInfo")] UserControl deviceInfo
        )
        {
            DevicesList = devicesList;
            DeviceInfo = deviceInfo;
        }

        public UserControl DevicesList { get; }

        public UserControl DeviceInfo { get; }
    }
}
