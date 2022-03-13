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
		private const int _OctetSize = 8;

		private const int _OctetsAmount = 4;

		private const int _MaxMask = 32;

		public IList<IPAddress> GenerateRange(string netwokAddress, int mask)
		{
			throw new NotImplementedException();
		}

		// private IPAddress GetFirstHost()
		// {
		// 	var result = new byte[_OctetsAmount];
		// 	for (int i = 0; i < _OctetsAmount; i++)
		// 		result[i] = (byte)(NetworkAddressOctets[i] & MaskOctets[i]);
		// 	if (result[result.Length - 1] < 255) result[result.Length - 1]++;

		// 	return new IPAddress { Address = GetAddressFromOctets(result), DeviceID = Guid.Empty };
		// }

		// private byte[] GetOctetsFromAddress(string address)
		// {
		// 	var addressParts = address.Split(".");
		// 	var result = new byte[_octetsAmount];

		// 	for (int i = 0; i < _octetsAmount; i++)
		// 		result[i] = byte.Parse(addressParts[i]);

		// 	return result;
		// }
	}
}
