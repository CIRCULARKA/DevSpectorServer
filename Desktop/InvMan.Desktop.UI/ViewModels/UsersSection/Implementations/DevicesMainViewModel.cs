using Avalonia.Controls;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class UsersMainViewModel : ViewModelBase, IUsersMainViewModel
    {
        public UsersMainViewModel(
            UsersListView usersList,
            SearchView search
        )
        {
            UsersList = usersList;
            Search = search;
        }

        public UserControl UsersList { get; }

        public UserControl Search { get; }
    }
}
