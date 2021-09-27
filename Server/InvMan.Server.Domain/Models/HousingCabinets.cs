namespace InvMan.Server.Domain.Models
{
	public class HousingCabinets
	{
		public int ID { get; set; }

		public int HousingID { get; set; }

		public Housing Housing { get; set; }

		public int CabinetID { get; set; }

		public Cabinet Cabinet { get; set;}
	}
}
