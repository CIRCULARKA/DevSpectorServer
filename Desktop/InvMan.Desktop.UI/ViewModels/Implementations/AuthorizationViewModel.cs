using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace InvMan.Desktop.UI.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase, IAuthorizationViewModel
    {
        private string _login;

        private string _password;

        public AuthorizationViewModel()
        {
            AuthorizationCommand = ReactiveCommand.CreateFromTask(
                () => Task.FromResult(new Unit())
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
