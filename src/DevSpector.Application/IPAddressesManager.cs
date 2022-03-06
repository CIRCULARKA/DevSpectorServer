using System.Linq;
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

		public void AddIPAddressToDevice(string inventoryNumber, string ipAddress)
		{
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

			var ip4Regexp = new Regex("\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}\b");
			if (ip4Regexp.IsMatch(ipAddress) )
		}

		public void RemoveIPAddressFromDevice(string inventoryNumber, string ipAddress)
		{
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

		}

	}
}
