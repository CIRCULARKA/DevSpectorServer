using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class UserInfoViewModel : ViewModelBase, IUserInfoViewModel
    {
        private string _accessToken;

        private string _login;

        private string _group;

        public UserInfoViewModel() { }

        public string AccessToken
        {
            get { return _accessToken == null ? "N/A" : _accessToken; }
            set => this.RaiseAndSetIfChanged(ref _accessToken, value);
        }

        public string Group
        {
            get { return _group == null ? "N/A" : _group; }
            set => this.RaiseAndSetIfChanged(ref _group, value);
        }

        public string Login
        {
            get { return _login == null ? "N/A" : _login; }
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public void UpdateUserInfo(User target)
        {
            AccessToken = target?.AccessToken;
            Group = target?.Group;
            Login = target?.Login;
        }
    }
}
