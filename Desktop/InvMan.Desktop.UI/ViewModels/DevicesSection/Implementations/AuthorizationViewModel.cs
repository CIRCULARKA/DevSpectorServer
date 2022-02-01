using System;
using System.Reactive;
using System.Net.Http;
using System.Threading;
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

        private string _errorMessage;

        private bool _attemptingToLogIn;

        private bool _logInFailed;

        private readonly IAuthorizationManager _authManager;

        private readonly IUserSession _session;

        public AuthorizationViewModel(IUserSession session)
        {
            _authManager = new AuthorizationManager();
            _session = session;

            AuthorizationCommand = ReactiveCommand.CreateFromTask(
                () => TryToAuthorize()
            );
        }

        public bool AttemptingToLogIn
        {
            get => _attemptingToLogIn;
            set => this.RaiseAndSetIfChanged(ref _attemptingToLogIn, value);
        }

        public bool LogInFailed
        {
            get => _logInFailed;
            set => this.RaiseAndSetIfChanged(ref _logInFailed, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
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
                LogInFailed = false;
                AttemptingToLogIn = true;

                // var accessToken = await _authManager.GetAccessTokenAsync(Login, Password);
                var accessToken = await _authManager.GetAccessTokenAsync("ruslan", "123Abc!");

                _session.StartSession(Login, accessToken);
            }
            catch (ArgumentException)
            {
                LogInFailed = true;

                ErrorMessage = "Логин или пароль введены неверно";

            }
            catch (HttpRequestException)
            {
                LogInFailed = true;

                ErrorMessage = "Не удалось подключиться к серверу";
            }
            catch
            {
                LogInFailed = true;

                ErrorMessage = "Что-то пошло не так :/";
            }
            finally { AttemptingToLogIn = false; }
        }
    }
}
