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
            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            desktop.MainWindow = _kernel.Get<AuthorizationView>();

            SubscribeToEvents();

            base.OnFrameworkInitializationCompleted();
        }

        private void SubscribeToEvents()
        {
            var appEvents = _kernel.Get<IApplicationEvents>();

            //
            // Get views
            //
            var mainView = _kernel.Get<MainView>();
            var authView = _kernel.Get<AuthorizationView>();

            //
            // Get VM's
            //
            // VM stands for View Model

            var authVM = _kernel.Get<IAuthorizationViewModel>();
            var commonInfoVM = _kernel.Get<ICommonInfoViewModel>();
            var locationInfoVM = _kernel.Get<ILocationInfoViewModel>();
            var softwareInfoVM = _kernel.Get<ISoftwareInfoViewModel>();
            var networkInfoVM = _kernel.Get<INetworkInfoViewModel>();
            var devicesListVM = _kernel.Get<IDevicesListViewModel>();
            var usersListVM = _kernel.Get<IUsersListViewModel>();
            var sessionBrokerVM = _kernel.Get<ISessionBrokerViewModel>();

            //
            // Subscribe VMs UpdateDeviceInfo on appliance selection
            //

            var targetVMsAmount = 4;
            var deviceInfoVMs = new List<IDeviceInfoViewModel>(targetVMsAmount);

            deviceInfoVMs.Add(commonInfoVM);
            deviceInfoVMs.Add(locationInfoVM);
            deviceInfoVMs.Add(softwareInfoVM);
            deviceInfoVMs.Add(networkInfoVM);

            foreach (var vm in deviceInfoVMs)
                appEvents.ApplianceSelected += vm.UpdateDeviceInfo;

            //
            // Subscribe appliances list update on search
            //

            appEvents.SearchExecuted += devicesListVM.LoadAppliances;

            //
            // Subscribe on authorization
            //

            appEvents.AuthorizationCompleted += mainView.Show;
            appEvents.AuthorizationCompleted += authView.Hide;
            appEvents.AuthorizationCompleted += devicesListVM.InitializeList;
            appEvents.AuthorizationCompleted += usersListVM.InitializeList;

            appEvents.UserAuthorized += sessionBrokerVM.UpdateLoggedUserInfo;

            //
            // Subscribe on logout
            //
            appEvents.Logout += mainView.Hide;
            appEvents.Logout += authView.Show;
            appEvents.Logout += authVM.ClearCredentials;
        }
    }
}
