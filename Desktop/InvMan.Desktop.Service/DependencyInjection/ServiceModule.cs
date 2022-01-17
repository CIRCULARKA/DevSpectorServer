using Ninject.Modules;

namespace InvMan.Desktop.Service.DependencyInjection
{
	public class ServicesModulue : NinjectModule
	{
		public override void Load()
		{
            Bind<IApplicationEvents>().To<ApplicationEvents>().InSingletonScope();
		}
	}
}
