using System.Collections.Generic;
using Xamarin.Forms;
using InvMan.Common.Models;

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
                new Appliance(1, "COMMUTATOR-1", "Коммутатор", "NSGK123123", null),
                new Appliance(2, "MAIN-SERVER", "Сервер", "NSGK032543", null),
                new Appliance(3, "COMMUTATOR-1", "Коммутатор", "NSGK3526901", null),
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
