using System.Collections.Generic;

namespace InvMan.Server.Domain.Models
{
	public class Device
	{
		public int ID { get; set; }

		public string InventoryNumber { get; set; }

		public DeviceType Type { get; set; }

		public string NetworkName { get; set; }

		public Location Location { get; set; }

		public List<IPAddress> IPAddresses { get; set; }
	}
}
