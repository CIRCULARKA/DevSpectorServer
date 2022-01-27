using Avalonia.Controls;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IMainViewModel
    {
        public UserControl DevicesList { get; }

        public UserControl SoftwareInfo { get; }

        public UserControl LocationInfo { get; }

        public UserControl NetworkInfo { get; }

        public UserControl CommonInfo { get; }

        public UserControl Search { get; }
    }
}
