using System.Collections.Generic;

namespace DevSpector.Application.Networking
{
	public interface IIPAddressesProvider
	{
		IEnumerable<string> GetFreeIP();

		IEnumerable<string> GetFreeIPSorted();

		void GenerateRange(string networkAddress, int mask);

		bool IsAddressFree(string ipAddress);
	}
}
