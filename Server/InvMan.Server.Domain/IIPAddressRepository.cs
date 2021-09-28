using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Domain
{
	public interface IIPAddressRepository
	{
		/// <summary>
		/// Replaces all existing IPs to new ones
		/// </summary>
		void UpdateIPs(IEnumerable<IPAddress> newIPs);

		IQueryable<IPAddress> IPAddresses { get; }

		// IQueryable<DeviceIPAddresses> DeviceIPAddresses { get; }
	}
}
