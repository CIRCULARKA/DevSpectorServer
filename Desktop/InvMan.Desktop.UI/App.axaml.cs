using Ninject;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.Service;
using InvMan.Desktop.UI.Views;
using InvMan.Desktop.UI.ViewModels;
using InvMan.Desktop.Service.DependencyInjection;

namespace InvMan.Desktop.UI
{
    public class App : Application
    {
        private readonly IKernel _kernel;

        public App()
        {
            _kernel = new StandardKernel(
                new ViewModelsModule(),
                new SdkModule(),
                new ServicesModulue()
            );
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            SubscribeToEvents();

            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            desktop.MainWindow = _kernel.Get<MainView>();
        }

        private void SubscribeToEvents()
        {
            var appEvents = _kernel.Get<IApplicationEvents>();

            var deviceInfoViewModel = _kernel.Get<IDeviceInfoViewModel>();

            appEvents.ApplianceSelected += deviceInfoViewModel.UpdateDeviceInformation;
        }
    }
}
