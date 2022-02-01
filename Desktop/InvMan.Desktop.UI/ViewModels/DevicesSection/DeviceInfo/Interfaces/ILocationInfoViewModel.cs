namespace InvMan.Desktop.UI.ViewModels
{
    public interface ILocationInfoViewModel : IDeviceInfoViewModel
    {
        string Housing { get; set; }

        string Cabinet { get; set; }
    }
}
