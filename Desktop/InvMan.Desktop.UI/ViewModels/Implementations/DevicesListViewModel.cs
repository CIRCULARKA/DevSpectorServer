using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ViewModelBase, IDevicesListViewModel
    {
        private readonly IDevicesProvider _devicesProvider;

        public DevicesListViewModel(IDevicesProvider devicesProvider)
        {
            _devicesProvider = devicesProvider;

            LoadAppliances();
        }

        public ObservableCollection<Appliance> Appliances { get; set; }

        private async void LoadAppliances()
        {
            Appliances = new ObservableCollection<Appliance>();

            var appliances = await _devicesProvider.GetDevicesAsync();
            foreach (var device in appliances)
                Appliances.Add(device);
        }
    }
}
