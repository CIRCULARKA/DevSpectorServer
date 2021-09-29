namespace InvMan.Server.Domain.Models
{
	public class Device
	{
		public int ID { get; set; }

		public string InventoryNumber { get; set; }

		public int TypeID { get; set; }

		public DeviceType Type { get; set; }

		public string NetworkName { get; set; }

		public int LocationID { get; set; }

		public Location Location { get; set; }
	}
}
