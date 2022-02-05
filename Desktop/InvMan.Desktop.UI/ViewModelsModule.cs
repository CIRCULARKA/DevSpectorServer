using Ninject.Modules;
using InvMan.Desktop.UI.Views;
using InvMan.Desktop.UI.ViewModels;

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
            Bind<IMainMenuViewModel>().To<MainMenuViewModel>().InSingletonScope();

            Bind<IDevicesMainViewModel>().To<DevicesMainViewModel>().InSingletonScope();

            Bind<IDevicesListViewModel>().To<DevicesListViewModel>().InSingletonScope();
            Bind<IUsersMainViewModel>().To<UsersMainViewModel>().InSingletonScope();

            Bind<ICommonInfoViewModel>().To<CommonInfoViewModel>().InSingletonScope();
            Bind<INetworkInfoViewModel>().To<NetworkInfoViewModel>().InSingletonScope();
            Bind<ISoftwareInfoViewModel>().To<SoftwareInfoViewModel>().InSingletonScope();
            Bind<ILocationInfoViewModel>().To<LocationInfoViewModel>().InSingletonScope();

            Bind<ISearchViewModel>().To<SearchViewModel>().InSingletonScope();

            Bind<IAuthorizationViewModel>().To<AuthorizationViewModel>().InSingletonScope();
        }

        private void BindViews()
        {
            Bind<MainView>().ToSelf().InSingletonScope();

            Bind<AuthorizationView>().ToSelf().InSingletonScope();

            Bind<DevicesMainView>().ToSelf().InSingletonScope();
            Bind<UsersMainView>().ToSelf().InSingletonScope();

            Bind<DevicesListView>().ToSelf();
            Bind<UsersListView>().ToSelf();

            Bind<CommonInfoView>().ToSelf();
            Bind<SoftwareInfoView>().ToSelf();
            Bind<LocationInfoView>().ToSelf();
            Bind<NetworkInfoView>().ToSelf();

            Bind<SearchView>().ToSelf();
        }
    }
}
