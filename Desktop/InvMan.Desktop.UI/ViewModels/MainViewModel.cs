using Avalonia.Controls;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(
            DevicesMainView devicesMainView,
            UsersMainView usersMainView,
            SessionBrokerView sessionBrokerView
        )
        {
            DevicesMainView = devicesMainView;
            UsersMainView = usersMainView;
            SessionBrokerView = sessionBrokerView;
        }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }

        public UserControl SessionBrokerView { get; }
    }
}
