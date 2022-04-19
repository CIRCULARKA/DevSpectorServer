using System;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	public class IPAddressEditor : IIPAddressEditor
	{
		private IRepository _repo;

		private IIPRangeGenerator _ipRangeGenerator;

		public IPAddressEditor(
			IRepository repo,
			IIPValidator ipValidator,
			IIPRangeGenerator ipRangeGenerator
		)
		{
			_repo = repo;
			_ipRangeGenerator = ipRangeGenerator;
		}

		public void GenerateRange(string networkAddress, int mask)
		{
			if (networkAddress == null)
				throw new ArgumentNullException("Сетевой адрес не может быть пустым");
			if (mask < 18 || mask > 31)
				throw new ArgumentException("Значение маски подсети не должно превышать 31 и быть меньше 18");

			// Get ip addresses according to mask and put them into new IPAddress objects
			var ips = _ipRangeGenerator.GenerateRange(networkAddress, mask);
			var newIps = new IPAddress[ips.Count];
			for (int i = 0; i < ips.Count; i++)
			{
				newIps[i] = new IPAddress {
					Address = ips[i]
				};
			}

			// Remove existing IP addresses from database
			var busyIps = _repo.Get<DeviceIPAddress>();
			_repo.RemoveRange(busyIps);
			_repo.Save();

			var existingIps = _repo.Get<IPAddress>();
			_repo.RemoveRange(existingIps);

			// Add newly generated IPs to database
			foreach (var ip in newIps)
				_repo.Add<IPAddress>(ip);

			_repo.Save();
		}
	}
}
