namespace InvMan.Server.Domain.Models
{
	public class Cabinet
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public int HousingID { get; set; }

		public Housing Housing { get; set; }
	}
}
