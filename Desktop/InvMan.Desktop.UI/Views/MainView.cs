using System.Net.Http;
using System.Windows;

namespace InvMan.Desktop.UI.Views
{
	public partial class MainView : Window
	{
		public MainView()
		{
			InitializeComponent();

			var client = new HttpClient();

			try
			{
				resultLabel.Text = client.GetAsync("http://localhost:5000/api/devices").Result.Content.ReadAsStringAsync().Result;
			}
			catch
			{
				resultLabel.Text = "Can not estabilish connection with server";
			}
		}
	}
}
