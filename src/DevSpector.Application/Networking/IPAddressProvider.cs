using System;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application.Networking
{
	public class IPAddressProvider : IIPAddressProvider
	{
		private IRepository _repo;

		private IIPValidator _ipValidator;

		private IIPRangeGenerator _ipRangeGenerator;

		public IPAddressProvider(
			IRepository repo,
			IIPValidator ipValidator,
			IIPRangeGenerator ipRangeGenerator
		)
		{
			_repo = repo;
			_ipValidator = ipValidator;
			_ipRangeGenerator = ipRangeGenerator;
		}

		public List<IPAddress> GetAllIP() =>
			_repo.Get<IPAddress>().ToList();

		public List<IPAddress> GetFreeIP() =>
			_repo.Get<IPAddress>(
				filter: ip => ip.DeviceID == null
			).ToList();

		public List<IPAddress> GetFreeIPSorted() =>
			_repo.Get<IPAddress>(
				filter: ip => ip.DeviceID == null
			).OrderBy(ip => int.Parse(ip.Address.Split(".")[0])).
				ThenBy(ip => int.Parse(ip.Address.Split(".")[1])).
				ThenBy(ip => int.Parse(ip.Address.Split(".")[2])).
				ThenBy(ip => int.Parse(ip.Address.Split(".")[3])).
					ToList();

		public IPAddress GetIP(string address) =>
			_repo.GetSingle<IPAddress>(ip => ip.Address == address);

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

			return GetFreeIP().Where(ip => ip.Address == ipAddress).Count() == 1;
		}
	}
}
