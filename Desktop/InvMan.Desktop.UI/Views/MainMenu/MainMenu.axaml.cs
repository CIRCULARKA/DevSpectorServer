using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using InvMan.Desktop.UI.ViewModels;

namespace InvMan.Desktop.UI.Views
{
    public partial class MainMenu : UserControl
    {
        private Button _mainMenuButton;

        private Button _devicesButton;

        private Button _usersButton;

        private List<Button> _menuOptions;

        private Grid _mainGrid;

        private ColumnDefinition _menuColumn;

        private readonly double _padding;

        private string _title;

        public MainMenu() { }

        public MainMenu(IMainMenuViewModel viewModel)
        {
            _padding = 15;

            _menuOptions = new List<Button>();

            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;

            _mainGrid = this.FindControl<Grid>("mainGrid");
            _mainMenuButton = this.FindControl<Button>("menuButton");
            _menuColumn = _mainGrid.ColumnDefinitions[0];

            _devicesButton = this.FindControl<Button>("devicesButton");
            _usersButton = this.FindControl<Button>("usersButton");

            _menuOptions.Add(_mainMenuButton);
            _menuOptions.Add(_devicesButton);
            _menuOptions.Add(_usersButton);

            _mainMenuButton.Content = Title;

            MenuToggle += ResizeButtons;
        }

        public event Action<double> MenuToggle;

        public bool IsMinimized => _menuColumn.Width.Value == MinMenuWidth;

        public double MinMenuWidth => 55;

        public double MaxMenuWidth => 250;

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public void ToggleMenu(object _, RoutedEventArgs info)
        {
            if (IsMinimized) Maximize();
            else Minimize();
        }

        public void Maximize()
        {
            _menuColumn.Width = new GridLength(MaxMenuWidth);
            MenuToggle?.Invoke(MaxMenuWidth);
        }

        public void Minimize()
        {
            _menuColumn.Width = new GridLength(MinMenuWidth);
            MenuToggle?.Invoke(MinMenuWidth);
        }

        private void ResizeButtons(double width)
        {
            foreach (var button in _menuOptions)
                button.Width = width - _padding;

            if (width == MinMenuWidth)
                foreach (var button in _menuOptions)
                    button.Content = "!!!";
        }
    }
}
