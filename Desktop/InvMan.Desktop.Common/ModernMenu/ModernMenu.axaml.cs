using Avalonia;
using Avalonia.Controls.Primitives;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenu : TemplatedControl
    {
        public static readonly AvaloniaProperty<string> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, string>(nameof(Title), "Title");

        public static readonly AvaloniaProperty<object> ContentProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Content));

        public ModernMenu() { }

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
    }
}
