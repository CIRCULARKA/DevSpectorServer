using System.Collections.Generic;

namespace DevSpector.Domain.Models
{
	public class IPAddressComparer : IEqualityComparer<IPAddress>
	{
		public bool Equals(IPAddress ip1, IPAddress ip2)
		{
			if (ip1 == null && ip2 == null)
				return true;

			if (ip1 != null || ip2 != null)
				return false;

			return ip1.Address == ip2.Address;
		}

		public int GetHashCode(IPAddress obj)
		{
			return base.GetHashCode();
		}
	}
}
