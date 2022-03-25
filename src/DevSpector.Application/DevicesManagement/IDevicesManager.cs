using System;
using System.Collections.Generic;
using DevSpector.Domain.Models;
using DevSpector.Database.DTO;

namespace DevSpector.Application.Devices
{
	public interface IDevicesManager
	{
		void CreateDevice(DeviceToAdd info);

		void UpdateDevice(string targetDeviceInventoryNumber, DeviceToAdd info);

		void DeleteDevice(string inventoryNumber);

		void MoveDevice(string inventoryNumber, Guid cabinetID);

		void AddSoftware(string inventoryNumber, SoftwareInfo info);

		void RemoveSoftware(string inventoryNumber, SoftwareInfo info);

		void AddIPAddress(string inventoryNumber, string ipAddress);

		void RemoveIPAddress(string inventoryNumber, string ipAddress);

		Cabinet GetDeviceCabinet(Guid deviceID);

		IEnumerable<IPAddress> GetIPAddresses(Guid deviceID);

		IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID);

		IEnumerable<Device> GetDevices();

		IEnumerable<DeviceToOutput> GetDevicesToOutput();

		IEnumerable<DeviceType> GetDeviceTypes();
	}
}
