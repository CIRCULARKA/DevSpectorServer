using System;

namespace DevSpector.Domain.Models
{
	public class DeviceCabinet
	{
		public Guid ID { get; set; }

		public Guid DeviceID { get; set; }

		public Guid CabinetID { get; set; }

		public Device Devcie { get; set; }

		public Cabinet Cabinet { get; set; }
	}
}
