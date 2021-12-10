using Ninject;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.UI.ViewModels;
using InvMan.Desktop.UI.Views;
using InvMan.Common.SDK;

namespace InvMan.Desktop.UI
{
    public class App : Application
    {
        private IKernel _kernel;

        public App(IKernel kernel)
        {
            _kernel = kernel;

            ConfigureServices();
        }

        private void ConfigureServices()
        {
            _kernel.Bind<IRawDataProvider>().To<JsonProvider>();
            _kernel.Bind<IDevicesProvider>().To<DevicesProvider>();
        }


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(
                        _kernel.Get<IRawDataProvider>(),
                        _kernel.Get<IDevicesProvider>()
                    ),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
