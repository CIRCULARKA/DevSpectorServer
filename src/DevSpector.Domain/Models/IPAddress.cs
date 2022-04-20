using System;
using System.Linq;
using System.Collections.Generic;

namespace DevSpector.Domain.Models
{
	public class IPAddress
	{
		public Guid ID { get; set; }

		public string Address { get; set; }

		public static List<IPAddress> SortIPs(List<IPAddress> ip) =>
			ip.OrderBy(ip => int.Parse(ip.Address.Split(".")[0])).
				ThenBy(ip => int.Parse(ip.Address.Split(".")[1])).
				ThenBy(ip => int.Parse(ip.Address.Split(".")[2])).
				ThenBy(ip => int.Parse(ip.Address.Split(".")[3])).
					ToList();
	}
}
