using System.Text;
using System.Linq;
using System.Collections.Generic;
using ReactiveUI;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class NetworkInfoViewModel : ViewModelBase, INetworkInfoViewModel
    {
        private string _networkName;

        private string _ipAddresses;

        public NetworkInfoViewModel() { }

        public string NetworkName
        {
            get { return _networkName == null ? "N/A" : _networkName; }
            set => this.RaiseAndSetIfChanged(ref _networkName, value);
        }

        public string IPAddresses
        {
            get { return _ipAddresses == null ? "Нет IP-адресов" : _ipAddresses; }
            set { this.RaiseAndSetIfChanged(ref _ipAddresses, value); }
        }

        public void UpdateDeviceInfo(Appliance target)
        {
            NetworkName = target?.NetworkName;

            IPAddresses = target == null ? null :
                CreateStringFromIP(target.IPAddresses);
        }

        private string CreateStringFromIP(IEnumerable<string> ips)
        {
            var ipCount = ips.Count();

            if (ipCount == 0)
                return "Нет IP-адресов";

            var newLines = ipCount;
            const int IpAddressMaxLength = 19;

            var builder = new StringBuilder(
                (ipCount * IpAddressMaxLength) + newLines
            );

            foreach (var ip in ips)
                builder.Append(ip).Append("\n");

            return builder.ToString();
        }
    }
}
