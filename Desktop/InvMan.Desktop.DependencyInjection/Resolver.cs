using Ninject;
using Ninject.Modules;

namespace InvMan.Desktop.DependencyInjection
{
	public static class Resolver
	{
		private static StandardKernel _kernel;

		static Resolver() =>
			_kernel = new StandardKernel();

		public static void AddModule(NinjectModule module) =>
			_kernel.Load(module);
	}
}
