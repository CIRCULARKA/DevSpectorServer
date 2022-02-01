using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace InvMan.Desktop.UI.Views.Shared
{
    public class ModernMenu : TemplatedControl
    {
        private object _currentContent;

        private Button _mainButton;

        private List<ModernMenuItem> _menuItems;

        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<ModernMenu, string>(nameof(Title), "Title");

        public static readonly DirectProperty<ModernMenu, List<ModernMenuItem>> MenuItemsProperty =
            AvaloniaProperty.RegisterDirect<ModernMenu, List<ModernMenuItem>>(nameof(MenuItems), o => o.MenuItems);

        public static readonly DirectProperty<ModernMenu, object> CurrentContentProperty =
            AvaloniaProperty.RegisterDirect<ModernMenu, object>(nameof(MenuItems), o => o.CurrentContent);

        public static readonly StyledProperty<int> SelectedIndexProperty =
            AvaloniaProperty.Register<ModernMenu, int>(nameof(SelectedIndex), 0);

        public ModernMenu()
        {
            _menuItems = new List<ModernMenuItem>();

            SelectedIndexProperty.Changed.Subscribe(ChangeCurrentContent);
        }

        private void ChangeCurrentContent(AvaloniaPropertyChangedEventArgs<int> info)
        {
            var incomingValue = info.NewValue.Value;
            if (incomingValue >= _menuItems.Count) return;

            CurrentContent = _menuItems[info.NewValue.Value].Content;
        }

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

        public List<ModernMenuItem> MenuItems => _menuItems;

        public void Add(ModernMenuItem item)
        {
            item.Index = MenuItems.Count;
            MenuItems.Add(item);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _mainButton = GetTemplateControl<Button>(e, "PART_mainButton");
        }

        protected async override void OnInitialized()
        {
            base.OnInitialized();

            // I don't know why but I found very weird solution about why CurrentContent gets default value of SelectedIndexProperty
            // instead of desired value that was set in the MainView.axaml
            // and I don't know how does it work
            // HELP MEEE
            await Task.Run(() => Thread.Sleep(1));

            CurrentContent = _menuItems[GetValue(SelectedIndexProperty)].Content;

            SubscribeMenuItemsClickEvent();
        }

        private void SelectIndex(object sender, RoutedEventArgs info)
        {
            var sourceButton = (sender as ModernMenuItem);

            SetValue(SelectedIndexProperty, sourceButton.Index);
        }

        private void SubscribeMenuItemsClickEvent()
        {
            foreach (var button in _menuItems)
                button.Click += SelectIndex;
        }

        private T GetTemplateControl<T>(TemplateAppliedEventArgs e, string controlName)
            where T : AvaloniaObject =>
            e.NameScope.Find<T>(controlName);
    }
}
