using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;
using ReactiveUI;

namespace InvMan.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ViewModelBase, IDevicesListViewModel
    {
        private readonly IDevicesProvider _devicesProvider;

        private string _noAppliancesMessage;

        private bool _areThereAppliances;

        private bool _areAppliancesLoaded;

        public DevicesListViewModel(IDevicesProvider devicesProvider)
        {
            _devicesProvider = devicesProvider;

            _areAppliancesLoaded = false;

            DefineViewContent();
        }

        public ObservableCollection<Appliance> Appliances { get; set; }

        public bool AreAppliancesLoaded
        {
            get => _areAppliancesLoaded;
            set => this.RaiseAndSetIfChanged(ref _areAppliancesLoaded, value);
        }

        public bool AreThereAppliances
        {
            get => _areThereAppliances;
            set { this.RaiseAndSetIfChanged(ref _areThereAppliances, value); }
        }

        public string NoAppliancesMessage
        {
            get => _noAppliancesMessage;
            set { this.RaiseAndSetIfChanged(ref _noAppliancesMessage, value); }
        }

        private async Task LoadAppliances()
        {
            AreAppliancesLoaded = false;

            Appliances = new ObservableCollection<Appliance>();

            var appliances = await _devicesProvider.GetDevicesAsync();
            foreach (var device in appliances)
                Appliances.Add(device);

        }

        private async void DefineViewContent()
        {
            try {
                await LoadAppliances();

                if (Appliances.Count > 0) {
                    AreThereAppliances = true;
                }
                else {
                    AreThereAppliances = false;
                    NoAppliancesMessage = "Нет устройств";
                }
            }
            catch
            {
                AreThereAppliances = false;
                NoAppliancesMessage = "Не удалось загрузить устройства из сервера";
            }
            finally { AreAppliancesLoaded = true; }
        }
    }
}
