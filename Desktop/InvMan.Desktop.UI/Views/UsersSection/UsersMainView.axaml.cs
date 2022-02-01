using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace InvMan.Desktop.UI.Views
{
    public partial class UsersMainView : UserControl
    {
        public UsersMainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
