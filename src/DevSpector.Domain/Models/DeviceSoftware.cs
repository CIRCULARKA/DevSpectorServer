using System;

namespace DevSpector.Domain.Models
{
	public class DeviceSoftware
	{
		public Guid ID { get; set; }

		public Guid DeviceID { get; set; }

		public Device Device { get; set; }

		public string SoftwareName { get; set; }

		public string SoftwareVersion { get; set; }
	}
}
