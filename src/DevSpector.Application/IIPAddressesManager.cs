using System.Collections.Generic;

namespace DevSpector.Application
{
	public interface IIPAddressesManager
	{
		IEnumerable<string> GetFreeIP();

		IEnumerable<string> GetSortedFreeIP();

		void AddIPAddressToDevice(string inventoryNumber, string ipAddress);

		void RemoveIPAddressFromDevice(string inventoryNumber, string ipAddress);

	}
}
