using System;
using System.Collections.Generic;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	/// <summary>
	/// Class for generating the range of IP addresses using specified mask
	/// </summary>
	public class IP4RangeGenerator
	{
		private readonly int _octetSize = 8;

		private readonly int _octetsAmount = 4;

		public IList<IPAddress> GenerateRange(string netwokAddress, int mask)
		{
			throw new NotImplementedException();
		}
	}
}
