using System;
using System.Collections.Generic;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;
using DevSpector.Database;

namespace DevSpector.Application
{
	public class DevicesManager : IDevicesManager
	{
		private readonly IDevicesProvider _devicesProvider;

		private readonly IDevicesEditor _devicesEditor;

		public DevicesManager(
			IDevicesProvider devicesProvider,
			IDevicesEditor devicesEditor
		)
		{
			_devicesProvider = devicesProvider;
			_devicesEditor = devicesEditor;
		}

		public void CreateDevice(DeviceInfo info) =>
			_devicesEditor.CreateDevice(info);

		public void UpdateDevice(string targetInventoryNumber, DeviceInfo info) =>
			_devicesEditor.UpdateDevice(targetInventoryNumber, info);

		public void DeleteDevice(string inventoryNumber) =>
			_devicesEditor.DeleteDevice(inventoryNumber);

		public void MoveDevice(string inventoryNumber, Guid cabinetID) =>
			_devicesEditor.MoveDevice(inventoryNumber, cabinetID);

		public void AddSoftware(string inventoryNumber, SoftwareInfo info) =>
			_devicesEditor.AddSoftware(inventoryNumber, info);

		public void RemoveSoftware(string inventoryNumber, SoftwareInfo info) =>
			_devicesEditor.RemoveSoftware(inventoryNumber, info);

		public IEnumerable<Device> GetDevices() =>
			_devicesProvider.GetDevices();

		public Cabinet GetDeviceCabinet(Guid deviceID) =>
			_devicesProvider.GetDeviceCabinet(deviceID);

		public IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID) =>
			_devicesProvider.GetDeviceSoftware(deviceID);

		public IEnumerable<Appliance> GetDevicesAsAppliances() =>
			_devicesProvider.GetDevicesAsAppliances();

		public IEnumerable<DeviceType> GetDeviceTypes() =>
			_devicesProvider.GetDeviceTypes();

		public IEnumerable<IPAddress> GetIPAddresses(Guid deviceID) =>
			_devicesProvider.GetIPAddresses(deviceID);

		public void AddIPAddress(string inventoryNumber, string ipAddress) =>
			_devicesEditor.AddIPAddress(inventoryNumber, ipAddress);

		public void RemoveIPAddress(string inventoryNumber, string ipAddress) =>
			_devicesEditor.RemoveIPAddress(inventoryNumber, ipAddress);
	}
}
