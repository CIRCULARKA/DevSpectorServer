using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public class UserSession : IUserSession
    {
        private readonly IApplicationEvents _events;

        private User _loggedUser;

        public UserSession(IApplicationEvents events)
        {
            _events = events;
        }

        public string Login => _loggedUser?.Login;

        public string Group => _loggedUser?.Group;

        public string AccessToken => _loggedUser?.AccessToken;

        public void StartSession(User user)
        {
            _loggedUser = user;

            _events.RaiseUserAuthorized(user);
            _events.RaiseAuthorizationCompleted();
        }
    }
}
