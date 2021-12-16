using System.Collections.Generic;

namespace InvMan.Server.Application
{
	public interface IIPAddressesManager
	{
		IEnumerable<string> GetFreeIP();

		IEnumerable<string> GetSortedFreeIP();
	}
}
