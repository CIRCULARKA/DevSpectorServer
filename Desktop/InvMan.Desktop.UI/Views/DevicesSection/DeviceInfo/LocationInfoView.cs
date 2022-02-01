using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.UI.ViewModels;

namespace InvMan.Desktop.UI.Views
{
    public partial class LocationInfoView : UserControl
    {
        public LocationInfoView() { }

        public LocationInfoView(ILocationInfoViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;
        }
    }
}
