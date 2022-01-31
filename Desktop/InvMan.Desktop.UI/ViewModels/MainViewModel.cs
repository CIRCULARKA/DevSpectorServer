using System.Reactive;
using System.Collections.Generic;
using Avalonia.Controls;
using ReactiveUI;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private UserControl _currentContent;

        public MainViewModel(
            DevicesMainView devicesMainView,
            UsersMainView usersMainView
        )
        {
            DevicesMainView = devicesMainView;
            UsersMainView = usersMainView;
            CurrentContent = DevicesMainView;
        }

        public UserControl CurrentContent
        {
            get => _currentContent;
            set { this.RaiseAndSetIfChanged(ref _currentContent, value); }
        }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }
    }
}
