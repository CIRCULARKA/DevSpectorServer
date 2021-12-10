using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Ninject;

namespace InvMan.Desktop.UI
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            BuildAvaloniaApp(kernel).
                StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp(KernelBase kernel) =>
            AppBuilder.Configure<App>(
                    () => new App(kernel)
                ).
                UsePlatformDetect().
                LogToTrace().
                UseReactiveUI();
    }
}
