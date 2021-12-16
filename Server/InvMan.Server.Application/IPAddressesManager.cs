using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Application
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
	}
}
