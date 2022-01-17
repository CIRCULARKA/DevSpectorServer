using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class DeviceInfoViewModel : ViewModelBase, IDeviceInfoViewModel
    {
        private string _networkName;

        public DeviceInfoViewModel() { }

        public string NetworkName
        {
            get { return _networkName == null ? "Устройство не выбрано" : _networkName; }
            set => this.RaiseAndSetIfChanged(ref _networkName, value);
        }

        public void UpdateDeviceInformation(Appliance target)
        {
            NetworkName = target.NetworkName;
        }
    }
}
