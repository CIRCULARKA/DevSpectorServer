using System;

namespace InvMan.Server.Domain.Models
{
	public class Location
	{
		public Guid ID { get; set; }

		public Guid HousingID { get; set; }

		public Housing Housing { get; set; }

		public Guid CabinetID { get; set; }

		public Cabinet Cabinet { get; set; }
	}
}
