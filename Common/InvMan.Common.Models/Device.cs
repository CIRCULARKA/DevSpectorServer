using System.Collections.Generic;

namespace InvMan.Common.Models
{
	public class Device
	{
		public int ID { get; init; }

		public string InventoryNumber { get; init; }

		public string Type { get; init; }

		public string NetworkName { get; init; }

		public List<string> IPAddresses { get; init; }
	}
}
