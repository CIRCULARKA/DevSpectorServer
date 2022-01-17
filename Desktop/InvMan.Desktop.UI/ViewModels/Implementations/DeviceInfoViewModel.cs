using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;
using ReactiveUI;

namespace InvMan.Desktop.UI.ViewModels
{
    public class DeviceInfoViewModel : ViewModelBase, IDeviceInfoViewModel
    {
        private string _networkName;

        public DeviceInfoViewModel()
        {
            NetworkName = "Temp";
        }

        public string NetworkName
        {
            get => _networkName;
            set => this.RaiseAndSetIfChanged(ref _networkName, value);
        }
    }
}
