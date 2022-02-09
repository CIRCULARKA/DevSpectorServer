using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IUserInfoViewModel
    {
        string AccessToken { get; set; }

        string Login { get; set; }

        string Group { get; set; }

        void UpdateUserInfo(User target);
    }
}
