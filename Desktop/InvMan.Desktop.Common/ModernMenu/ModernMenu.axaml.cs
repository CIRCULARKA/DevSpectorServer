using Avalonia;
using Avalonia.Controls.Primitives;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenu : TemplatedControl
    {
        public static readonly AvaloniaProperty<string> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, string>(nameof(Title), "Title");

        public static readonly AvaloniaProperty<object> CurrentContentProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(CurrentContent));

        public ModernMenu() { }

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public object CurrentContent
        {
            get => GetValue(CurrentContentProperty);
            set => SetValue(CurrentContentProperty, value);
        }
    }
}
