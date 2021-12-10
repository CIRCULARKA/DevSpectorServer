using Ninject.Modules;
using InvMan.Desktop.UI.ViewModels;

namespace InvMan.Desktop.Service.DependencyInjection
{
    public class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindowViewModel>().ToSelf();
        }
    }
}
