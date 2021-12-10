using Ninject;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using InvMan.Desktop.UI.ViewModels;
using InvMan.Desktop.UI.Views;
using InvMan.Desktop.Service.DependencyInjection;

namespace InvMan.Desktop.UI
{
    public class App : Application
    {
        private readonly IKernel _kernel;

        public App()
        {
            _kernel = new StandardKernel(
                new ViewModelsModule(),
                new SdkModule(),
                new RootModule()
            );
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            desktop.MainWindow = _kernel.Get<MainView>();
        }
    }
}
