using System.Collections.Generic;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	public interface IIPAddressProvider
	{
		IEnumerable<IPAddress> GetFreeIP();

		IEnumerable<IPAddress> GetFreeIPSorted();

		IPAddress GetIP(string address);

		void GenerateRange(string networkAddress, int mask);

		bool IsAddressFree(string ipAddress);
	}
}
