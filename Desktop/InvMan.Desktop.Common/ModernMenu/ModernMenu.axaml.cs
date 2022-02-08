using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace InvMan.Desktop.UI.Views.Shared
{
    public partial class ModernMenu : TemplatedControl
    {
        private object _currentContent;

        private Button _mainButton;

        private ModernMenuItem _selectedItem;

        private ColumnDefinition _menuColumn;

        private Path _minimizedMenuIcon;

        private Path _maximizedMenuIcon;

        private List<ModernMenuItem> _allMenuItems;

        public ModernMenu()
        {
            _menuItems = new List<ModernMenuItem>();
            _bottomItems = new List<ModernMenuItem>();
            _allMenuItems = new List<ModernMenuItem>();

            SelectedIndexProperty.Changed.Subscribe(ChangeCurrentContent);
        }

        private void ChangeCurrentContent(AvaloniaPropertyChangedEventArgs<int> info)
        {
            var incomingValue = info.NewValue.Value;
            if (incomingValue >= _allMenuItems.Count) return;

            CurrentContent = _allMenuItems[info.NewValue.Value].Content;
        }

        public void Add(ModernMenuItem item)
        {
            item.Index = _allMenuItems.Count;

            item.Initialized += (o, info) => {
                if (SelectedIndex == item.Index)
                    CurrentContent = item.Content;
            };

            _allMenuItems.Add(item);
        }

        public void MainMenuButtonClicked(object _, RoutedEventArgs info) =>
            ToggleMenuSize();

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _mainButton = GetTemplateControl<Button>(e, "PART_mainButton");
            _mainButton.Click += MainMenuButtonClicked;

            _menuColumn = GetTemplateControl<Grid>(e, "PART_menuContainer").ColumnDefinitions[0];

            _minimizedMenuIcon = Application.Current.FindResource("minimizedMenuIcon") as Path;
            _maximizedMenuIcon = Application.Current.FindResource("maximizedMenuIcon") as Path;

            ToggleMenuSize();

            foreach (var item in _allMenuItems)
                if (item.IsBottom)
                    BottomMenuItems.Add(item);
                else
                    MenuItems.Add(item);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SubscribeMenuItemsClickEvent();
        }

        private bool IsMinimized => _menuColumn.Width.Value == MinMenuSize;

        private void ToggleMenuSize()
        {
            if (IsMinimized)
                MaximizeMenu();
            else
                MinimizeMenu();

            ToggleMenuOptionsContent();
        }

        private void MinimizeMenu() =>
            _menuColumn.Width = new GridLength(MinMenuSize);

        private void MaximizeMenu() =>
            _menuColumn.Width = new GridLength(MaxMenuSize);

        private void ToggleMenuOptionsContent()
        {
            if (IsMinimized)
            {
                foreach (var button in _allMenuItems)
                    button.MinimizeTitle();
                _mainButton.Content = _minimizedMenuIcon;
            }
            else
            {
                foreach (var button in _allMenuItems)
                    button.MaximizeTitle();
                _mainButton.Content = "Меню";
            }
        }

        private void SelectIndex(object sender, RoutedEventArgs info)
        {
            _selectedItem = sender as ModernMenuItem;

            SelectedIndex = _selectedItem.Index;
        }

        private void SubscribeMenuItemsClickEvent()
        {
            foreach (var button in _allMenuItems)
                button.Click += SelectIndex;
        }

        private T GetTemplateControl<T>(TemplateAppliedEventArgs e, string controlName)
            where T : AvaloniaObject =>
            e.NameScope.Find<T>(controlName);
    }
}
