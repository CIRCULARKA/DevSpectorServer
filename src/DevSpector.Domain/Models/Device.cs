using System;

namespace DevSpector.Domain.Models
{
	public class Device
	{
		public Guid ID { get; set; }

		public Guid TypeID { get; set; }

		public DeviceType Type { get; set; }

		public string InventoryNumber { get; set; }

		public string NetworkName { get; set; }
	}
}
