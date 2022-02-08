using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface ISessionBrokerViewModel
    {
        string LoggedUserLogin { get; set; }

        void UpdateLoggedUserInfo(User user);
    }
}
