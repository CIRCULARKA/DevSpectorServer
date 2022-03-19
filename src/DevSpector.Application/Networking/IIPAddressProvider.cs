using System.Collections.Generic;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	public interface IIPAddressProvider
	{
		List<IPAddress> GetAllIP();

		List<IPAddress> GetFreeIP();

		List<IPAddress> GetFreeIPSorted();

		IPAddress GetIP(string address);

		void GenerateRange(string networkAddress, int mask);

		bool IsAddressFree(string ipAddress);
	}
}
