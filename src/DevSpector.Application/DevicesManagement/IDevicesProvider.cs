using System;
using System.Collections.Generic;
using DevSpector.Database;
using DevSpector.SDK.Models;
using DevSpector.Domain.Models;

namespace DevSpector.Application
{
	public interface IDevicesProvider
	{
		IEnumerable<Device> GetDevices();

		IEnumerable<DeviceType> GetDeviceTypes();

		IEnumerable<IPAddress> GetIPAddresses(Guid deviceID);

		IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID);

		IEnumerable<Appliance> GetDevicesAsAppliances();

		/// <summary>
		/// Returns all versions of specified software name
		/// </summary>
		IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID, string softwareName);

		Cabinet GetDeviceCabinet(Guid deviceID);

		DeviceSoftware GetDeviceSoftware(Guid deviceID, string softwareName, string softwareVersion);

        bool DoesDeviceExist(string inventoryNumber);

        bool IsNetworkNameUnique(string networkName);

        bool DoesDeviceTypeExist(Guid typeID);

		bool HasSoftware(Guid deviceID, string softwareName);

		bool HasSoftware(Guid deviceID, string softwareName, string softwareVersion);
	}
}
