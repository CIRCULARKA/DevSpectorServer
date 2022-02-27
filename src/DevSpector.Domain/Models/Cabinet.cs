using System;

namespace DevSpector.Domain.Models
{
	public class Cabinet
	{
		public Guid ID { get; set; }

		public Guid HousingID { get; set; }

		public Housing Housing { get; set; }

		public string Name { get; set; }
	}
}
