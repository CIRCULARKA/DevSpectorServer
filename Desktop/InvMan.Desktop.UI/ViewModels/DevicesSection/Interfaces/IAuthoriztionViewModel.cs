using System.Reactive;
using ReactiveUI;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IAuthorizationViewModel
    {
        public ReactiveCommand<Unit, Unit> AuthorizationCommand { get; }

        public string Login { get; set; }

        public string Password { get; set; }

        public void ClearCredentials();
    }
}
