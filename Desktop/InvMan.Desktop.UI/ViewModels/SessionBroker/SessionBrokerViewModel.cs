using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class SessionBrokerViewModel : ViewModelBase, ISessionBrokerViewModel
    {
        private string _loggedUserLogin;

        private string _loggedUserGroup;

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
