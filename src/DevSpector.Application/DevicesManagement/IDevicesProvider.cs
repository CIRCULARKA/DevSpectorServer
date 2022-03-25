using System;
using System.Collections.Generic;
using DevSpector.Domain.Models;
using DevSpector.Database.DTO;

namespace DevSpector.Application.Devices
{
	public interface IDevicesProvider
	{
		List<Device> GetDevices();

		List<DeviceType> GetDeviceTypes();

		List<IPAddress> GetIPAddresses(Guid deviceID);

		List<DeviceSoftware> GetDeviceSoftware(Guid deviceID);

		List<DeviceToOutput> GetDevicesToOutput();

		/// <summary>
		/// Returns all versions of specified software name
		/// </summary>
		List<DeviceSoftware> GetDeviceSoftware(Guid deviceID, string softwareName);

		Cabinet GetDeviceCabinet(Guid deviceID);

		DeviceSoftware GetDeviceSoftware(Guid deviceID, string softwareName, string softwareVersion);

		Device GetDevice(string inventoryNumber);

        bool DoesDeviceExist(string inventoryNumber);

        bool IsNetworkNameUnique(string networkName);

        bool DoesDeviceTypeExist(Guid typeID);

		bool HasSoftware(Guid deviceID, string softwareName);

		bool HasSoftware(Guid deviceID, string softwareName, string softwareVersion);

		bool HasIP(Guid deviceID, string ipAddress);
	}
}
