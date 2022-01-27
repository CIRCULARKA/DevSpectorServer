using System;
using System.Collections.Generic;

namespace InvMan.Common.SDK.Models
{
	public class Appliance
	{
		public Appliance(Guid id, string inventoryNumber, string type,
			string networkName, string housing, string cabinet,
			List<string> ipAddresses, List<string> software)
		{
			ID = id;
			InventoryNumber = inventoryNumber;
			Type = type;
			NetworkName = networkName;
			Housing = housing;
			Cabinet = cabinet;
			IPAddresses = ipAddresses;
			Software = software;
		}

		public Guid ID { get; }

		public string InventoryNumber { get; }

		public string Type { get; }

		public string NetworkName { get; }

		public string Housing { get; }

		public string Cabinet { get; }

		public List<string> IPAddresses { get; }

		public List<string> Software { get; }
	}
}
