using Avalonia;
using Avalonia.Controls;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenuItem : Button
    {
        private object _title;

        public static readonly DirectProperty<ModernMenuItem, object> TitleProperty =
            AvaloniaProperty.RegisterDirect<ModernMenuItem, object>(
                nameof(Title),
                o => o.Title
            );

        public static readonly StyledProperty<object> RegularTitleProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(RegularTitle), "Menu item title");

        public static readonly StyledProperty<object> MinimizedTitleProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(MinimizedTitle), "T");

        public new static readonly StyledProperty<object> ContentProperty =
            AvaloniaProperty.Register<ModernMenu, object>(nameof(Content), "Content should be here");

        public ModernMenuItem()
        {
            // RegularTitleProperty.Changed.Subscribe(
            //     o => Title = o.NewValue
            // );
        }

        public int Index { get; set; }

        public object RegularTitle
        {
            get => GetValue(RegularTitleProperty);
            set => SetValue(RegularTitleProperty, value);
        }

        public object Title
        {
            get => _title;
            private set => SetAndRaise(TitleProperty, ref _title, value);
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

        public void MinimizeTitle() =>
            Title = MinimizedTitle;

        public void MaximizeTitle() =>
            Title = RegularTitle;
    }
}
