using Ninject.Modules;
using InvMan.Common.SDK;

namespace InvMan.Desktop.Service.DependencyInjection
{
	public class SdkModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IRawDataProvider>().To<JsonProvider>();
			Bind<IDevicesProvider>().To<DevicesProvider>();

			Bind<IUserSession>().To<UserSession>().InSingletonScope();
		}
	}
}
