using System.Collections.Generic;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Networking
{
	/// <summary>
	/// Class for generating the range of IP addresses using specified mask
	/// </summary>
	public interface IIPRangeGenerator
	{
		IList<string> GenerateRange(string netwokAddress, int mask);
	}
}
