namespace InvMan.Desktop.Service
{
    public class UserSession : IUserSession
    {
        private readonly IApplicationEvents _events;

        public UserSession(IApplicationEvents events)
        {
            _events = events;
        }

        public string Login { get; private set; }

        public string AccessToken { get; private set; }

        public void StartSession(string login, string acessToken)
        {
            Login = login;
            AccessToken = acessToken;

            _events.RaiseUserAuthorized();
        }
    }
}
