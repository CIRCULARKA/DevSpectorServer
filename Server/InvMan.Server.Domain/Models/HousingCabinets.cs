using System.Collections.Generic;

namespace InvMan.Server.Domain.Models
{
	public class HousingCabinets
	{
		public int ID { get; set; }

		public Housing Housing { get; set; }

		public IEnumerable<Cabinet> Cabinets { get; set; }
	}
}
