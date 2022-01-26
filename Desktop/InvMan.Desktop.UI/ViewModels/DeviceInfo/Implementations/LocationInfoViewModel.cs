using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class LocationInfoViewModel : ViewModelBase, ILocationInfoViewModel
    {
        private string _housing;

        private string _cabinet;

        public LocationInfoViewModel() { }

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

        public void UpdateDeviceInfo(Appliance target)
        {
            Housing = target.Housing;
            Cabinet = target.Cabinet;
        }
    }
}
