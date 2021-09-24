using System.Collections.Generic;

namespace InvMan.Common.Models
{
	public class Appliance
	{
		public Appliance(int id, string inventoryNumber, string type,
			string networkName, IEnumerable<string> ipAddresses)
		{
			ID = id;
			InventoryNumber = inventoryNumber;
			Type = type;
			NetworkName = networkName;
			IPAddresses = ipAddresses;
		}

		public int ID { get; private set; }

		public string InventoryNumber { get; private set; }

		public string Type { get; private set; }

		public string NetworkName { get; private set; }

		public IEnumerable<string> IPAddresses { get; private set; }
	}
}
