using System.Collections.Generic;
using Avalonia;

namespace InvMan.Desktop.UI.Views.Shared
{
    public partial class ModernMenu
    {
        private List<ModernMenuItem> _menuItems;

        private List<ModernMenuItem> _bottomItems;

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, string>(nameof(Title), "Title");

        public static readonly DirectProperty<ModernMenu, List<ModernMenuItem>> MenuItemsProperty =
            AvaloniaProperty.RegisterDirect<ModernMenu, List<ModernMenuItem>>(nameof(MenuItems), o => o.MenuItems);

        public static readonly DirectProperty<ModernMenu, object> CurrentContentProperty =
            AvaloniaProperty.RegisterDirect<ModernMenu, object>(nameof(MenuItems), o => o.CurrentContent);

        public static readonly StyledProperty<int> SelectedIndexProperty =
            AvaloniaProperty.Register<ModernMenu, int>(nameof(SelectedIndex), 0);

        public static readonly StyledProperty<double> MinMenuSizeProperty =
            AvaloniaProperty.Register<ModernMenu, double>(nameof(MinMenuSize), 70.0);

        public static readonly StyledProperty<double> MaxMenuSizeProperty =
            AvaloniaProperty.Register<ModernMenu, double>(nameof(MaxMenuSize), 250.0);

        public static readonly StyledProperty<bool> StartMinimizedProperty =
            AvaloniaProperty.Register<ModernMenu, bool>(nameof(StartMinimized), false);

        public static readonly DirectProperty<ModernMenu, List<ModernMenuItem>> BottomMenuItemsProperty =
            AvaloniaProperty.RegisterDirect<ModernMenu, List<ModernMenuItem>>(
                nameof(BottomMenuItems),
                o => o.BottomMenuItems
            );

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public object CurrentContent
        {
            get => _currentContent;
            set => SetAndRaise<object>(CurrentContentProperty, ref _currentContent, value);
        }

        public double MinMenuSize
        {
            get => GetValue(MinMenuSizeProperty);
            set => SetValue(MinMenuSizeProperty, value);
        }

        public double MaxMenuSize
        {
            get => GetValue(MaxMenuSizeProperty);
            set => SetValue(MaxMenuSizeProperty, value);
        }

        public bool StartMinimized
        {
            get => GetValue(StartMinimizedProperty);
            set => SetValue(StartMinimizedProperty, value);
        }

        public List<ModernMenuItem> BottomMenuItems => _bottomItems;

        public List<ModernMenuItem> MenuItems => _menuItems;
    }
}
