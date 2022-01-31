using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenu : TemplatedControl
    {
        private Button _mainButton;

        private List<ModernMenuItem> _menuItems;

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, string>(nameof(Title), "Title");

        public static readonly DirectProperty<ModernMenu, List<ModernMenuItem>> MenuItemsProperty =
            AvaloniaProperty.RegisterDirect<ModernMenu, List<ModernMenuItem>>(nameof(MenuItems), o => o.MenuItems);

        public ModernMenu()
        {
            _menuItems = new List<ModernMenuItem>();
        }

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public List<ModernMenuItem> MenuItems => _menuItems;

        public void Add(ModernMenuItem item)
        {
            MenuItems.Add(item);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _mainButton = GetTemplateControl<Button>(e, "PART_mainButton");
        }

        private T GetTemplateControl<T>(TemplateAppliedEventArgs e, string controlName)
            where T : AvaloniaObject =>
            e.NameScope.Find<T>(controlName);
    }
}
