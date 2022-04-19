using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application.Networking
{
	/// <summary>
	/// Class for generating the range of IP addresses using specified mask
	/// </summary>
	public class IP4RangeGenerator : IIPRangeGenerator
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

		public IList<string> GenerateRange(string netwokAddress, int mask)
		{
			if (!_ipValidator.Matches(netwokAddress, IPProtocol.Version4))
				throw new ArgumentException("Сетевой адрес не соответствует шаблону IPv4");

			SetCurrentMask(mask);
			SetCurrentLocalAddressBytes(mask);

			var hostsAmount = (int)Math.Pow(2, _MaxMask - mask) - 2;

			var firstHostBytes = GetFirstHostBytes(netwokAddress);
			var lastHostBytes = GetLastHostBytes(netwokAddress);

			var result = new List<string>(hostsAmount);

			var currentIP = firstHostBytes;

			result.Add(GetAddressFromOctets(currentIP));

			for (int i = 0; i < hostsAmount - 1; i++)
				for (int j = _OctetsAmount - 1; j > 0; j--)
					if (firstHostBytes[j] < lastHostBytes[j])
					{
						currentIP[j]++;
						result.Add(GetAddressFromOctets(currentIP));
						break;
					}

			return result;
		}

		private byte[] GetFirstHostBytes(string address)
		{
			if (!_ipValidator.Matches(address, IPProtocol.Version4))
				throw new ArgumentException("Невозможно получить первый хост диапазона - IP-адрес не соответствует шаблону IPv4");

			var networkAddressOctets = GetOctetsFromAddress(address);

			var result = new byte[_OctetsAmount];
			for (int i = 0; i < _OctetsAmount; i++)
				result[i] = (byte)(networkAddressOctets[i] & _currentLocalAddressBytes[i]);
			if (result[result.Length - 1] < 255) result[result.Length - 1]++;

			return result;
		}

		private byte[] GetLastHostBytes(string address)
		{
			if (!_ipValidator.Matches(address, IPProtocol.Version4))
				throw new ArgumentException("Невозможно получить последний хост диапазона - IP-адрес не соответствует шаблону IPv4");

			var networkAddressOctets = GetOctetsFromAddress(address);

			var result = new byte[_OctetsAmount];
			for (int i = 0; i < _OctetsAmount; i++)
				result[i] = (byte)(networkAddressOctets[i] | ~_currentLocalAddressBytes[i]);
			if (result[result.Length - 1] != 0) result[result.Length - 1]--;

			return result;
		}

		private byte[] GetLocalAddressBytes(int mask)
		{
			uint maskValue = ~(uint.MaxValue >> mask);
			return BitConverter.GetBytes(maskValue).Reverse().ToArray();
		}

		private byte[] GetOctetsFromAddress(string address)
		{
			if (!_ipValidator.Matches(address, IPProtocol.Version4))
				throw new ArgumentException("IP-адрес не соответствует шаблону IPv4");

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
				throw new ArgumentException("Значение макси должно быть между 20 и 30");
		}
	}
}
