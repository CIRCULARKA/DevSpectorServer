using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class DeviceInfoViewModel : ViewModelBase, IDeviceInfoViewModel
    {
        private string _networkName;

        private string _inventoryNumber;

        private string _type;

        private string _housing;

        private string _cabinet;

        public DeviceInfoViewModel() { }

        public string NetworkName
        {
            get { return _networkName == null ? "N/A" : _networkName; }
            set => this.RaiseAndSetIfChanged(ref _networkName, value);
        }

        public string InventoryNumber
        {
            get { return _inventoryNumber == null ? "N/A" : _inventoryNumber; }
            set => this.RaiseAndSetIfChanged(ref _inventoryNumber, value);
        }

        public string Type
        {
            get { return _type == null ? "N/A" : _type; }
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        public string Housing
        {
            get { return _housing == null ? "N/A" : _housing; }
            set => this.RaiseAndSetIfChanged(ref _housing, value);
        }

        public string Cabinet
        {
            get { return _cabinet == null ? "N/A" : _cabinet; }
            set => this.RaiseAndSetIfChanged(ref _cabinet, value);
        }

        public void UpdateDeviceInformation(Appliance target)
        {
            NetworkName = target.NetworkName;

            InventoryNumber = target.InventoryNumber;

            Type = target.Type;

            Housing = target.Housing;
            Cabinet = target.Cabinet;
        }
    }
}
