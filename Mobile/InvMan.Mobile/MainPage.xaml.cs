using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;

namespace InvMan.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly Uri _host;

        private readonly IRawDataProvider _jsonProvider;

        private readonly DevicesProvider _devicesProvider;

        public MainPage()
        {
            _host = new Uri("http://10.0.0.2:5000");
            _jsonProvider = new JsonProvider(_host);
            _devicesProvider = new DevicesProvider(_jsonProvider);

            InitializeComponent();

            Devices = _devicesProvider.GetDevicesAsync(3).Result.ToList();
        }

        public IEnumerable<Appliance> Devices { get; set; }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Appliance selectedАppliance = e.Item as Appliance;
            if (selectedАppliance != null)
                await DisplayAlert("Выбранное устройство", $"{selectedАppliance.Type} - {selectedАppliance.InventoryNumber}", "OK");
        }
	}
}
