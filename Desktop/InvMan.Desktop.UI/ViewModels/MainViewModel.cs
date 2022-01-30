using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private UserControl _currentContent;

        public MainViewModel(
            MainMenu mainMenu,
            DevicesMainView devicesMainView,
            UsersMainView usersMainView
        )
        {
            MainMenu = mainMenu;
            DevicesMainView = devicesMainView;
            UsersMainView = usersMainView;
            CurrentContent = DevicesMainView;
        }

        public ReactiveCommand<string, Unit> ChangeContentCommand { get; }

        public UserControl CurrentContent
        {
            get => _currentContent;
            set { this.RaiseAndSetIfChanged(ref _currentContent, value); }
        }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }

        public UserControl MainMenu { get; }
    }
}
