using System.Collections.Generic;
using Ninject;
using Avalonia;
using Avalonia.Controls;
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
            SubscribeToEvents();

            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            desktop.MainWindow = _kernel.Get<MainView>();

            // Unsuccesfull try of using hot reload. Will try later. Maybe
            // desktop.MainWindow = new LiveViewHost(this, Console.WriteLine);

            // var startupWindow = desktop.MainWindow as LiveViewHost;
            // startupWindow.StartWatchingSourceFilesForHotReloading();
            // startupWindow.Show();

            // RxApp.DefaultExceptionHandler = Observer.Create<Exception>(Console.WriteLine);

            base.OnFrameworkInitializationCompleted();
        }

        private void SubscribeToEvents()
        {
            var appEvents = _kernel.Get<IApplicationEvents>();

            // VM stands for View Model
            var targetVMsAmount = 4;
            var deviceInfoVMs = new List<IDeviceInfoViewModel>(targetVMsAmount);

            deviceInfoVMs.Add(_kernel.Get<ICommonInfoViewModel>());
            deviceInfoVMs.Add(_kernel.Get<ILocationInfoViewModel>());
            deviceInfoVMs.Add(_kernel.Get<ISoftwareInfoViewModel>());
            deviceInfoVMs.Add(_kernel.Get<INetworkInfoViewModel>());

            foreach (var vm in deviceInfoVMs)
                appEvents.ApplianceSelected += vm.UpdateDeviceInfo;
        }
    }
}
