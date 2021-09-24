using System.Collections.Generic;

namespace InvMan.Common.Models
{
	public class Appliance
	{
		public Appliance(int id, string inventoryNumber, string type,
			string networkName, string housing, string cabinet,
			IEnumerable<string> ipAddresses)
		{
			ID = id;
			InventoryNumber = inventoryNumber;
			Type = type;
			NetworkName = networkName;
			Housing = housing;
			Cabinet = cabinet;
			IPAddresses = ipAddresses;
		}

		public int ID { get; private set; }

		public string InventoryNumber { get; private set; }

		public string Type { get; private set; }

		public string NetworkName { get; private set; }

		public string Housing { get; private set; }

		public string Cabinet { get; private set; }

		public IEnumerable<string> IPAddresses { get; private set; }
	}
}
