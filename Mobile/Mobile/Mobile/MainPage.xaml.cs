using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using System.Net.Http.Headers;
using Mobile.Infrastructure;
using InvMan.Common.SDK;


namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public List<Аppliance> Аppliances { get; set; }
        
        public MainPage()
        {
            InitializeComponent();

           

            Аppliances = new List<Аppliance>
            {
                new Аppliance { Name  = "COMMUTATOR-1", DeviceType = "Коммутатор", InventoryNumber = "NSGK123123"},
                new Аppliance { Name  = "MAIN-SERVER", DeviceType = "Сервер", InventoryNumber = "NSGK052674"},
                new Аppliance { Name  = "IVAN-PC", DeviceType = "Персональный компьютер", InventoryNumber = "NSGK998784"},
            };
            this.BindingContext = this;
            
        }

        public async void OnItemTapped(object sende, ItemTappedEventArgs e)
        {
            Аppliance selectedАppliance = e.Item as Аppliance;
            if (selectedАppliance != null)
                await DisplayAlert("Выбранное устройство", $"{ selectedАppliance.DeviceType } - { selectedАppliance.InventoryNumber }", "OK");
        }        
       
    }

}
