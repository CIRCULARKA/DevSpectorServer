using System.Collections.Generic;

namespace DevSpector.Application
{
	public interface IIPAddressesManager
	{
		IEnumerable<string> GetFreeIP();

		IEnumerable<string> GetSortedFreeIP();
	}
}