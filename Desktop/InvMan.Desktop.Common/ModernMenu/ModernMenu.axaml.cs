using Avalonia;
using Avalonia.Controls.Primitives;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenu : TemplatedControl
    {
        public static readonly AvaloniaProperty<string> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, string>("Title", "Menu Title");

        public ModernMenu() { }

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }
    }
}
