using Ninject.Modules;
using InvMan.Common.SDK;

namespace InvMan.Desktop.Service.DependencyInjection
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IRawDataProvider>().To<JsonProvider>();
        }
    }
}
