using System.Linq;
using System.Collections.Generic;
using InvMan.Common.Models;

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

		public static implicit operator Appliance(Device d) =>
			new Appliance(d.ID, d.InventoryNumber, d.Type.Name, d.NetworkName,
				d.IPAddresses.Select(ip => ip.Address));
	}
}
