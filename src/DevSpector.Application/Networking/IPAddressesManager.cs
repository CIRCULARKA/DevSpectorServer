using System;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking.Enumerations;

namespace DevSpector.Application.Networking
{
	public class IPAddressesManager : IIPAddressesManager
	{
		private IRepository _repo;

		private IIPValidator _ipValidator;

		private IIPRangeGenerator _ipRangeGenerator;

		public IPAddressesManager(
			IRepository repo,
			IIPValidator ipValidator,
			IIPRangeGenerator ipRangeGenerator
		)
		{
			_repo = repo;
			_ipValidator = ipValidator;
			_ipRangeGenerator = ipRangeGenerator;
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
			// Get ip addresses according to mask and put them into new IPAddress objects
			var ips = _ipRangeGenerator.GenerateRange(networkAddress, mask);

			var newIps = new IPAddress[ips.Count];
			for (int i = 0; i < ips.Count; i++)
			{
				newIps[i] = new IPAddress {
					Address = ips[i],
					DeviceID = null
				};
			}

			// Remove existing IP addresses from database and add new ones
			var existingIps = _repo.Get<IPAddress>();
			foreach (var ip in existingIps)
				_repo.Remove<IPAddress>(ip);
			_repo.Save();

			foreach (var ip in newIps)
				_repo.Add<IPAddress>(ip);

			_repo.Save();
		}

		public bool IsAddressFree(string ipAddress)
		{
			if (!_ipValidator.Matches(ipAddress, IPProtocol.Version4))
				throw new ArgumentException("IP address does not match IPv4 pattern");

			return GetFreeIP().Contains(ipAddress);
		}
	}
}
