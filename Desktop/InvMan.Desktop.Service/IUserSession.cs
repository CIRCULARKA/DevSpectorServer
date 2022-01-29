namespace InvMan.Desktop.Service
{
    public interface IUserSession
    {
        string Login { get; }

        string AccessToken { get; }

        void StartSession(string login, string accessToken);
    }
}
