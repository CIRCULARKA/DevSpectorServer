using Avalonia.Controls;
using Ninject;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(
            [Named("DevicesList")] UserControl devicesList,
            [Named("DeviceInfo")] UserControl deviceInfo,
            [Named("Search")] UserControl search
        )
        {
            DevicesList = devicesList;
            DeviceInfo = deviceInfo;
            Search = search;
        }

        public UserControl DevicesList { get; }

        public UserControl DeviceInfo { get; }

        public UserControl Search { get; }
    }
}
