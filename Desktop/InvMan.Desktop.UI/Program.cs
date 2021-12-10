using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Ninject;
using InvMan.Desktop.Service.DependencyInjection;

namespace InvMan.Desktop.UI
{
	class Program
	{
		private static IKernel _kernel;

		[STAThread]
		public static void Main(string[] args)
		{
			_kernel = new StandardKernel(
                new RootModule(),
                new ViewModelsModule(),
                new SdkModule()
            );

			BuildAvaloniaApp().
				StartWithClassicDesktopLifetime(args);
		}

		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder.Configure<App>(
				() => new App() { Kernel = _kernel }
			).
				UsePlatformDetect().
				LogToTrace().
				UseReactiveUI();
	}
}
