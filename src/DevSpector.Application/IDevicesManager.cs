using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.Domain.Models;
using DevSpector.Database;

namespace DevSpector.Application
{
	public interface IDevicesManager
	{
		void CreateDevice(DeviceInfo info);

		void UpdateDevice(string targetDeviceInventoryNumber, DeviceInfo info);

		void DeleteDevice(string inventoryNumber);

		void MoveDevice(string inventoryNumber, Guid cabinetID);

		void AddSoftware(string inventoryNumber, SoftwareInfo info);

		void RemoveSoftware(string inventoryNumber, SoftwareInfo info);

		void AddIPAddressToDevice(string inventoryNumber, string ipAddress);

		void RemoveIPAddressFromDevice(string inventoryNumber, string ipAddress);

		void ThrowIfDevice(EntityExistance existance, string inventoryNumber);

		Cabinet GetDeviceCabinet(string inventoryNumber);

		IEnumerable<IPAddress> GetIPAddresses(Guid deviceID);

		IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID);

		IEnumerable<Device> GetDevices();

		IEnumerable<Appliance> GetAppliances();

		IEnumerable<DeviceType> GetDeviceTypes();
	}
}
