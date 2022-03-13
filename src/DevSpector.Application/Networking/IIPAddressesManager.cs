using System.Collections.Generic;

namespace DevSpector.Application
{
	public interface IIPAddressesManager
	{
		IEnumerable<string> GetFreeIP();

		IEnumerable<string> GetSortedFreeIP();

		void GenerateRange(string networkAddress, int mask);

		bool IsAddressFree(string ipAddress);

		bool MathesIPv4(string ipAddress);
	}
}
