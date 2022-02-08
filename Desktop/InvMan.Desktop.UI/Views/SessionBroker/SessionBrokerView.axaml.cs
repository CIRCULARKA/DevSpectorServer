using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.UI.ViewModels;

namespace InvMan.Desktop.UI.Views
{
    public partial class SessionBrokerView : UserControl
    {
        public SessionBrokerView() { }

        public SessionBrokerView(ISessionBrokerViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;
        }
    }
}
