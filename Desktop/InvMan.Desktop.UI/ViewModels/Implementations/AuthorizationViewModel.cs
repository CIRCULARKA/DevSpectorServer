using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using InvMan.Desktop.Service;
using InvMan.Common.SDK.Authorization;

namespace InvMan.Desktop.UI.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase, IAuthorizationViewModel
    {
        private string _login;

        private string _password;

        private readonly IAuthorizationManager _authManager;

        public AuthorizationViewModel()
        {
            _authManager = new AuthorizationManager();

            AuthorizationCommand = ReactiveCommand.CreateFromTask(
                () => TryToAuthorize()
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

        public async Task TryToAuthorize()
        {
            try
            {
                var accessToken = await _authManager.GetAccessTokenAsync(Login, Password);
            }
            catch (ArgumentException)
            {

            }
        }
    }
}
