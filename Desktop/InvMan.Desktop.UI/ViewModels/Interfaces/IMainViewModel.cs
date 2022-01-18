using Avalonia.Controls;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IMainViewModel
    {
        UserControl DevicesList { get; }

        UserControl DeviceInfo { get; }
    }
}
