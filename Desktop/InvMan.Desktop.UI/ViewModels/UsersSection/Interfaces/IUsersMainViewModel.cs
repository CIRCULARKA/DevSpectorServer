using Avalonia.Controls;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IUsersMainViewModel
    {
        UserControl UsersList { get; }

        UserControl Search { get; }
    }
}
