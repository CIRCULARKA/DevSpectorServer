using Avalonia.Controls;
using Ninject;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(
            [Named(nameof(DevicesList))] UserControl devicesList,
            [Named(nameof(CommonInfoView))] UserControl commonInfo,
            [Named(nameof(NetworkInfoView))] UserControl networkInfo,
            [Named(nameof(LocationInfoView))] UserControl locationInfo,
            [Named(nameof(SoftwareInfoView))] UserControl softwareInfo,
            [Named(nameof(SearchView))] UserControl search
        )
        {
            DevicesList = devicesList;
            Search = search;
        }

        public UserControl DevicesList { get; }

        public UserControl SoftwareInfo { get; }

        public UserControl LocationInfo { get; }

        public UserControl NetworkInfo { get; }

        public UserControl CommonInfo { get; }

        public UserControl Search { get; }
    }
}
