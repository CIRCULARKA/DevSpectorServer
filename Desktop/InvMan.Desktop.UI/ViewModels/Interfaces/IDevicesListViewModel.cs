using System.Collections.ObjectModel;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IDevicesListViewModel
    {
        ObservableCollection<Appliance> Appliances { get; set; }

        bool AreAppliancesLoaded { get; set; }

        bool AreThereAppliances { get; set; }

        string NoAppliancesMessage { get; set; }
    }
}
