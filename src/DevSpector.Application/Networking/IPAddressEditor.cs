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
	}
}
