using System;

namespace DevSpector.Domain.Models
{
	public class DeviceIPAddresses
	{
		public Guid ID { get; set; }

		public Guid DeviceID { get; set; }

		public Guid IPAddressID { get; set; }

		public Device Device { get; set; }

		public IPAddress IPAddress { get; set; }
	}
}
