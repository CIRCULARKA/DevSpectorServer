using Ninject.Modules;
using InvMan.Desktop.UI.Views;
using InvMan.Desktop.UI.ViewModels;
using Avalonia.Controls;

namespace InvMan.Desktop.Service.DependencyInjection
{
    public class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            BindViewModels();
            BindViews();
        }

        private void BindViewModels()
        {
            Bind<IMainViewModel>().To<MainViewModel>();
            Bind<IDevicesListViewModel>().To<DevicesListViewModel>();
            Bind<IDeviceInfoViewModel>().To<DeviceInfoViewModel>();
        }

        private void BindViews()
        {
            Bind<MainView>().ToSelf();
            Bind<UserControl>().To<DevicesList>().Named("DevicesList");
            Bind<UserControl>().To<DeviceInfo>().Named("DeviceInfo");
        }
    }
}
