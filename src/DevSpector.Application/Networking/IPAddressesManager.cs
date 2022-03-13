using System;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	public class IPAddressesManager : IIPAddressesManager
	{
		private IRepository _repo;

		private IIPValidator _ipValidator;

		public IPAddressesManager(
			IRepository repo,
			IIPValidator ipValidator
		)
		{
			_repo = repo;
			_ipValidator = ipValidator;
		}

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
			if (!_ipValidator.Matches(networkAddress))
				throw new ArgumentException("Network address does not match IPv4 pattern");

			if (mask < 20 && mask > 30)
				throw new ArgumentException("Mask should be in range from 20 to 30");
		}

		public bool IsAddressFree(string ipAddress)
		{
			if (!_ipValidator.Matches(ipAddress))
				throw new ArgumentException("IP address does not match IPv4 pattern");

			return GetFreeIP().Contains(ipAddress);
		}
	}
}
