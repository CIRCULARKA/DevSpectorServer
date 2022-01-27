using System.Collections.Generic;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface ISoftwareInfoViewModel : IDeviceInfoViewModel
    {
        string Software { get; set; }
    }
}
