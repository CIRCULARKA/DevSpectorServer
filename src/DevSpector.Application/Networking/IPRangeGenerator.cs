using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking.Enumerations;

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

		private int _currentMask;

		private byte[] _currentLocalAddressBytes;

		private readonly IIPValidator _ipValidator;

		public IP4RangeGenerator(IIPValidator ipValidator)
		{
			_ipValidator = ipValidator;
		}

		public IList<IPAddress> GenerateRange(string netwokAddress, int mask)
		{
			SetCurrentMask(mask);

			var firstHost = GetFirstHost(netwokAddress);

			throw new NotImplementedException();
		}

		private IPAddress GetFirstHost(string address)
		{
			if (_ipValidator.Matches(address, IPProtocol.Version4))
				throw new ArgumentException("Can't process first host - specified address doesn't match IPv4 pattern");

			var networkAddressOctets = GetOctetsFromAddress(address);

			var result = new byte[_OctetsAmount];
			for (int i = 0; i < _OctetsAmount; i++)
				result[i] = (byte)(networkAddressOctets[i] & _currentLocalAddressBytes[i]);
			if (result[result.Length - 1] < 255) result[result.Length - 1]++;

			return new IPAddress { Address = GetAddressFromOctets(result), DeviceID = Guid.Empty };
		}

		private IPAddress GetLastHost(string address)
		{
			if (_ipValidator.Matches(address, IPProtocol.Version4))
				throw new ArgumentException("Can't process last host - specified address doesn't match IPv4 pattern");

			var networkAddressOctets = GetOctetsFromAddress(address);

			var result = new byte[_OctetsAmount];
			for (int i = 0; i < _OctetsAmount; i++)
				result[i] = (byte)(networkAddressOctets[i] | ~_currentLocalAddressBytes[i]);
			if (result[result.Length - 1] != 0) result[result.Length - 1]--;

			return new IPAddress { Address = GetAddressFromOctets(result) };
		}

		private byte[] GetLocalAddressBytes(int mask)
		{
			uint maskValue = ~(uint.MaxValue >> mask);
			return BitConverter.GetBytes(maskValue).Reverse().ToArray();
		}

		private byte[] GetOctetsFromAddress(string address)
		{
			if (!_ipValidator.Matches(address, IPProtocol.Version4))
				throw new ArgumentException("IP address does not mathes IPv4 pattern");

			var addressParts = address.Split(".");
			var result = new byte[_OctetsAmount];

			for (int i = 0; i < _OctetsAmount; i++)
				result[i] = byte.Parse(addressParts[i]);

			return result;
		}

		private string GetAddressFromOctets(byte[] octets)
		{
			var dotsAmount = 3;
			var ipAddressStringSize = _OctetSize * _OctetsAmount + dotsAmount;

			var result = new StringBuilder(ipAddressStringSize);
			for (int i = 0; i < _OctetsAmount; i++)
			{
				result.Append(octets[i]);
				if (i != _OctetsAmount - 1) result.Append('.');
			}

			return result.ToString();
		}

		private void SetCurrentLocalAddressBytes(int mask)
		{
			ValidateMask(mask);

			_currentLocalAddressBytes = GetLocalAddressBytes(mask);
		}

		private void SetCurrentMask(int mask)
		{
			ValidateMask(mask);

			_currentMask = mask;
		}

		private void ValidateMask(int mask)
		{
			if (mask < 20 || mask > 30)
				throw new ArgumentException("Mask value must be between 20 and 30");
		}
	}
}
