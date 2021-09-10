using System.Collections.Generic;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Domain
{
	public interface IIPAddressRepository
	{
		IEnumerable<IPAddress> GetDeviceIPs(int deviceID);

		IEnumerable<IPAddress> FreeAddresses { get; }
	}
}
