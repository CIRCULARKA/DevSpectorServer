using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class SessionBrokerViewModel : ViewModelBase, ISessionBrokerViewModel
    {
        private string _loggedUserLogin;

        public string LoggedUserLogin
        {
            get => _loggedUserLogin;
            set => this.RaiseAndSetIfChanged(ref _loggedUserLogin, value);
        }

        public void UpdateLoggedUserInfo(User user)
        {
            LoggedUserLogin = user.Login;
        }
    }
}
