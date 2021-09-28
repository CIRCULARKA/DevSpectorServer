using System;
using System.Windows;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI
{
	public partial class MainWindow : Window
	{
		private readonly IRawDataProvider _rawDataProvider;

		private readonly DevicesProvider _devicesProvider;

		private readonly Uri _hostUri;

		public MainWindow()
		{
			_hostUri = new Uri("http://localhost:5000");
			_rawDataProvider = new JsonProvider(_hostUri);
			_devicesProvider = new DevicesProvider(_rawDataProvider);

			InitializeComponent();
		}

		public async void LoadDevicesAsync(object s, RoutedEventArgs info)
		{
			devices.ItemsSource = await _devicesProvider.GetDevicesAsync(3);
		}
	}
}
