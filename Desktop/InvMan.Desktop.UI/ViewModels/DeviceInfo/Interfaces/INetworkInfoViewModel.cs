namespace InvMan.Desktop.UI.ViewModels
{
    public interface INetworkInfoViewModel : IDeviceInfoViewModel
    {
        string IPAddresses { get; set; }

        string NetworkName { get; set; }
    }
}
