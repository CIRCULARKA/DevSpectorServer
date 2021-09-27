namespace InvMan.Server.Domain.Models
{
	public class DeviceIPAddresses
	{
		public int ID { get; set; }

		public int DeviceID { get; set; }

		public Device Device { get; set; }

		public int IPAddressID { get; set; }

		public IPAddress IPAddress { get; set; }
	}
}
