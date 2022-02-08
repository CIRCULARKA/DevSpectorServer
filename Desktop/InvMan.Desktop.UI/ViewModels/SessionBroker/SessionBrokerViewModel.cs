using System.Reactive;
using ReactiveUI;
using InvMan.Common.SDK.Models;
using InvMan.Desktop.Service;

namespace InvMan.Desktop.UI.ViewModels
{
    public class SessionBrokerViewModel : ViewModelBase, ISessionBrokerViewModel
    {
        private string _loggedUserLogin;

        private string _loggedUserGroup;

        private readonly IApplicationEvents _appEvents;

        public SessionBrokerViewModel(IApplicationEvents events)
        {
            _appEvents = events;

            LogoutCommand = ReactiveCommand.Create(
                () => _appEvents.RaiseLogout()
            );
        }

        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public string LoggedUserLogin
        {
            get => _loggedUserLogin;
            set => this.RaiseAndSetIfChanged(ref _loggedUserLogin, value);
        }

        public string LoggedUserGroup
        {
            get => _loggedUserGroup;
            set => this.RaiseAndSetIfChanged(ref _loggedUserGroup, value);
        }

        public void UpdateLoggedUserInfo(User user)
        {
            LoggedUserLogin = user.Login;
            LoggedUserGroup = user.Group;
        }
    }
}
