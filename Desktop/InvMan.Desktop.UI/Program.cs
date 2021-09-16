using System;
using System.Windows;
using InvMan.Desktop.DependencyInjection;

namespace InvMan.Desktop.UI
{
	public class Program
	{
		[STAThread]
		public static void Main()
		{
			try
			{
				var app = new App();
				app.Run();
			}
			catch (Exception e)
			{
				MessageBox.Show(
					"Unhandeld error: " + e.Message,
					"Error: program can't keep working",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}
	}
}
