namespace InvMan.Server.Domain.Models
{
	public class Location
	{
		public int ID { get; set; }

		public Housing Housing { get; set; }

		public Cabinet Cabinet { get; set; }
	}
}
