using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface ICommonInfoViewModel : IDeviceInfoViewModel
    {
        string InventoryNumber { get; set; }

        string Type { get; set; }
    }
}
