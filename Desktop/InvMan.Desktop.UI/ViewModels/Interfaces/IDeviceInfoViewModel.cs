using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IDeviceInfoViewModel
    {
        string NetworkName { get; set; }

        void UpdateDeviceInformation(Appliance target);
    }
}
