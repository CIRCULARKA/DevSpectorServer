using Avalonia;
using Avalonia.Controls;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenuItem : Button
    {
        public static readonly StyledProperty<object> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Title), "Menu item");

        public static readonly StyledProperty<object> MinimizedTitleProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Content), "T");

        public new static readonly StyledProperty<object> ContentProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Content), "Content should be here");

        public ModernMenuItem() { }

        public int Index { get; set; }

        public object Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public new object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public object MinimizedTitle
        {
            get => GetValue(MinimizedTitleProperty);
            set => SetValue(MinimizedTitleProperty, value);
        }
    }
}
