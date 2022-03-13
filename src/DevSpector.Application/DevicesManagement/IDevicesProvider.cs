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

		Cabinet GetDeviceCabinet(Guid deviceID);

        bool DoesDeviceExist(string inventoryNumber);

        bool IsNetworkNameUnique(string networkName);

        bool DoesDeviceTypeExist(Guid typeID);

		bool HasSoftware(Guid deviceID, string softwareName);

		bool HasSoftware(Guid deviceID, string softwareName, string softwareVersion);
	}
}
