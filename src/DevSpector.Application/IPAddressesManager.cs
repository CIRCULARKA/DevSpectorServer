using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application
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

		public bool IsAddressFree(string ipAddress)
		{
			if (!MathesIPv4(ipAddress))
				throw new ArgumentException("IP address does not match IPv4 pattern");

			return GetFreeIP().Contains(ipAddress);
		}

		public bool MathesIPv4(string ipAddress)
		{
			var ip4Pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
			return Regex.IsMatch(ipAddress, ip4Pattern);
		}
	}
}
