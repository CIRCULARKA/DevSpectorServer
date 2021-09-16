using System.Windows;
using InvMan.Desktop.UI.Views;

namespace InvMan.Desktop.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs info)
        {
            base.OnStartup(info);

            var mainView = new MainView();
            mainView.Show();
        }
    }
}
