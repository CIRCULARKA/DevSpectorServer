using Avalonia.Controls;
using Ninject;

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
