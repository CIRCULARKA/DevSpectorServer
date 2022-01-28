using System.Reactive;
using ReactiveUI;
using InvMan.Desktop.Service;

namespace InvMan.Desktop.UI.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase, IAuthorizationViewModel
    {
        private string _login;

        private string _password;

        private readonly IApplicationEvents _events;

        public AuthorizationViewModel(IApplicationEvents events)
        {
            _events = events;

            AuthorizationCommand = ReactiveCommand.Create(
                () => _events.RaiseUserAuthorized()
            );
        }

        public ReactiveCommand<Unit, Unit> AuthorizationCommand { get; }

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
    }
}
