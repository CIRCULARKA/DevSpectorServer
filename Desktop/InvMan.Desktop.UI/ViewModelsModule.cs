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
            Bind<IMainViewModel>().To<MainViewModel>().InSingletonScope();
            Bind<IDevicesListViewModel>().To<DevicesListViewModel>().InSingletonScope();
            Bind<IDeviceInfoViewModel>().To<DeviceInfoViewModel>().InSingletonScope();
            Bind<ISearchViewModel>().To<SearchViewModel>().InSingletonScope();
        }

        private void BindViews()
        {
            Bind<MainView>().ToSelf();
            Bind<UserControl>().To<DevicesList>().Named("DevicesList");
            Bind<UserControl>().To<DeviceInfo>().Named("DeviceInfo");
            Bind<UserControl>().To<Search>().Named("Search");
        }
    }
}
