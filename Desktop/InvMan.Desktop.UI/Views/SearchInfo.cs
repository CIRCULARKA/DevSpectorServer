using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.UI.ViewModels;

namespace InvMan.Desktop.UI.Views
{
    public partial class SearchView: UserControl
    {
        public SearchView() { }

        public SearchView(ISearchViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;
        }
    }
}
