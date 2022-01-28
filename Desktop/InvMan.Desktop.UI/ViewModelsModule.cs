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

            Bind<ICommonInfoViewModel>().To<CommonInfoViewModel>().InSingletonScope();
            Bind<INetworkInfoViewModel>().To<NetworkInfoViewModel>().InSingletonScope();
            Bind<ISoftwareInfoViewModel>().To<SoftwareInfoViewModel>().InSingletonScope();
            Bind<ILocationInfoViewModel>().To<LocationInfoViewModel>().InSingletonScope();

            Bind<ISearchViewModel>().To<SearchViewModel>().InSingletonScope();

            Bind<IAuthorizationViewModel>().To<AuthorizationViewModel>().InSingletonScope();
        }

        private void BindViews()
        {
            Bind<MainView>().ToSelf();
            Bind<AuthorizationView>().ToSelf();

            Bind<UserControl>().To<DevicesListView>().Named(nameof(DevicesListView));

            Bind<UserControl>().To<CommonInfoView>().Named(nameof(CommonInfoView));
            Bind<UserControl>().To<SoftwareInfoView>().Named(nameof(SoftwareInfoView));
            Bind<UserControl>().To<LocationInfoView>().Named(nameof(LocationInfoView));
            Bind<UserControl>().To<NetworkInfoView>().Named(nameof(NetworkInfoView));

            Bind<UserControl>().To<SearchView>().Named(nameof(SearchView));
        }
    }
}
