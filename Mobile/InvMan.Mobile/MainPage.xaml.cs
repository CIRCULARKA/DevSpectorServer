using System.Collections.Generic;
using Xamarin.Forms;
using InvMan.Common.SDK.Models;

namespace InvMan.Mobile
{
    public partial class MainPage : ContentPage
    {
        public List<Appliance> Devices { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Devices = new List<Appliance>
            {
                new Appliance(
                    id: 1,
                    networkName: "COMMUTATOR-1",
                    type: "Коммутатор",
                    inventoryNumber: "NSKG123123",
                    housing: "hous1",
                    cabinet: "cab1",
                    ipAddresses: null
                ),
                new Appliance(
                    id: 1,
                    networkName: "IVAN-PC",
                    type: "ПК",
                    inventoryNumber: "NSGK60231",
                    housing: "hous2",
                    cabinet: "cab2",
                    ipAddresses: null
                ),
                new Appliance(
                    id: 1,
                    networkName: "MAIN-SERVER",
                    type: "Сервер",
                    inventoryNumber: "NSGK53412e",
                    housing: "hous3",
                    cabinet: "cab3",
                    ipAddresses: null
                ),
            };
            this.BindingContext = this;
        }

        public async void OnItemTapped(object sende, ItemTappedEventArgs e)
        {
            Appliance selectedАppliance = e.Item as Appliance;
            if (selectedАppliance != null)
                await DisplayAlert("Выбранное устройство", $"{selectedАppliance.Type} - {selectedАppliance.InventoryNumber}", "OK");
        }
    }
}
