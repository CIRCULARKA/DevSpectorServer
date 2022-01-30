using Avalonia.Controls;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        public MainViewModel(
            MainMenu mainMenu
        ) { MainMenu = mainMenu; }

        public UserControl MainMenu { get; }
    }
}
