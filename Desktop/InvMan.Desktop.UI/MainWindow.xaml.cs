using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using InvMan.Common.SDK;

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
			var client = new HttpClient();
			client.Timeout = new TimeSpan(0, 0, 15);
			_rawDataProvider = new JsonProvider(_hostUri, client);
			_devicesProvider = new DevicesProvider(_rawDataProvider);

			InitializeComponent();
		}

		public async void LoadDevicesAsync(object s, RoutedEventArgs info)
		{
			downloadCircle.Visibility = Visibility.Visible;
			await Task.Run(() => Thread.Sleep(4000));
			try { devices.ItemsSource = await _devicesProvider.GetDevicesAsync(3); }
			catch { }
			downloadCircle.Visibility = Visibility.Collapsed;
		}
	}
}
