using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	public class IPAddressesManager : IIPAddressesManager
	{
		private IRepository _repo;

		public IPAddressesManager(IRepository repo) =>
			_repo = repo;

		public IEnumerable<string> GetFreeIP() =>
			_repo.Get<IPAddress>(
				filter: ip => ip.DeviceID == null
			).Select(ip => ip.Address);

		public IEnumerable<string> GetSortedFreeIP() =>
			_repo.Get<IPAddress>(
				filter: ip => ip.DeviceID == null
			).Select(ip => ip.Address).
				OrderBy(address => int.Parse(address.Split(".")[0])).
					ThenBy(address => int.Parse(address.Split(".")[1])).
					ThenBy(address => int.Parse(address.Split(".")[2])).
					ThenBy(address => int.Parse(address.Split(".")[3]));

		public void GenerateRange(string networkAddress, int mask)
		{
			if (!MathesIPv4(networkAddress))
				throw new ArgumentException("Network address does not match IPv4 pattern");

			if (mask < 20 && mask > 30)
				throw new ArgumentException("Mask should be in range from 20 to 30");
		}

		public bool IsAddressFree(string ipAddress)
		{
			if (!MathesIPv4(ipAddress))
				throw new ArgumentException("IP address does not match IPv4 pattern");

			return GetFreeIP().Contains(ipAddress);
		}

		public bool MathesIPv4(string ipAddress)
		{
			var ip4Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
			return Regex.IsMatch(ipAddress, ip4Pattern);
		}
	}
}
