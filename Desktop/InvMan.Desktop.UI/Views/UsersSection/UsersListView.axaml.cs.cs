using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.UI.ViewModels;

namespace InvMan.Desktop.UI.Views
{
    public partial class UsersListView : UserControl
    {
        public UsersListView() { }

        public UsersListView(IUsersListViewModel viewModel)
        {
            DataContext = viewModel;

            AvaloniaXamlLoader.Load(this);
        }
    }
}
