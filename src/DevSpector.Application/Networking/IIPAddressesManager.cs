using System.Collections.Generic;

namespace DevSpector.Application.Networking
{
	public interface IIPAddressesManager
	{
		IEnumerable<string> GetFreeIP();

		IEnumerable<string> GetSortedFreeIP();

		void GenerateRange(string networkAddress, int mask);

		bool IsAddressFree(string ipAddress);
	}
}
