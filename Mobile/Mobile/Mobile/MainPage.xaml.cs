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
                new Аppliance { Name  = "COMMUTATOR-1", Device_type = "Коммутатор", Inventory_number = "NSGK123123"},
                new Аppliance { Name  = "MAIN-SERVER", Device_type = "Сервер", Inventory_number = "NSGK052674"},
                new Аppliance { Name  = "IVAN-PC", Device_type = "Персональный компьютер", Inventory_number = "NSGK998784"},
            };
            this.BindingContext = this;

            /*LoadData();*/
        }

        public async void OnItemTapped(object sende, ItemTappedEventArgs e)
        {
            Аppliance selectedАppliance = e.Item as Аppliance;
            if (selectedАppliance != null)
                await DisplayAlert("Выбранное устройство", $"{ selectedАppliance.Device_type } - { selectedАppliance.Inventory_number }", "OK");
        }

        
        /*public async void LoadData()
        {
            var client = new HttpClient();

            Task<string> task = (await client.GetAsync("http://10.0.2.2:5000/api/devices")).Content.ReadAsStringAsync();

            result.Text = await task;
        }*/
    }

}
