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

		public List<IPAddress> GetFreeIP()
		{
			IEnumerable<IPAddress> allIps = _repo.Get<IPAddress>();
			IEnumerable<IPAddress> busyIps = _repo.Get<DeviceIPAddress>().Select(di => di.IPAddress);

			return allIps.Except(busyIps, new IPAddressComparer()).ToList();
		}

		public List<IPAddress> GetFreeIPSorted() =>
			IPAddress.SortIPs(GetFreeIP());

		public IPAddress GetIP(string address) =>
			_repo.GetSingle<IPAddress>(ip => ip.Address == address);

		public bool IsAddressFree(string ipAddress)
		{
			if (!_ipValidator.Matches(ipAddress, IPProtocol.Version4))
				throw new ArgumentException("IP-адрес не соответствует шаблону IPv4");

			return GetFreeIP().Where(ip => ip.Address == ipAddress).Count() == 1;
		}
	}
}
