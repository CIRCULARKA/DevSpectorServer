using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IDevicesListViewModel
    {
        ObservableCollection<Appliance> Appliances { get; set; }

        Appliance SelectedAppliance { get; set; }

        IEnumerable<Appliance> CachedDevices { get; }

        bool AreAppliancesLoaded { get; set; }

        bool AreThereAppliances { get; set; }

        string NoAppliancesMessage { get; set; }

        void LoadAppliances(IEnumerable<Appliance> devices);

        void InitializeList();
    }
}
