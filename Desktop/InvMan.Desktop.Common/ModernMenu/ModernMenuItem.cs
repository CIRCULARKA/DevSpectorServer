using Avalonia;
using Avalonia.Controls.Primitives;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenuItem : TemplatedControl
    {
        public static readonly StyledProperty<object> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Title), "Menu item");

        public static readonly StyledProperty<object> ContentProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Content), "Content should be here");

        public ModernMenuItem() { }

        public object Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
    }
}
