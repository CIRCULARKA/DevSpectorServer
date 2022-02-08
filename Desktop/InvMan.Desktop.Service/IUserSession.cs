using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public interface IUserSession
    {
        string Login { get; }

        string AccessToken { get; }

        void StartSession(User user);
    }
}
