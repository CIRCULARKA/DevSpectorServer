using System;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Database;

namespace DevSpector.Application.Devices
{
	public class DevicesEditor : IDevicesEditor
	{
		private IRepository _repo;

		private IDevicesProvider _devicesProvider;

		public DevicesEditor(
			IRepository repo,
			IDevicesProvider devicesProvider
		)
		{
			_repo = repo;
			_devicesProvider = devicesProvider;
		}

		public void CreateDevice(DeviceInfo info)
		{
			if (_devicesProvider.DoesDeviceExist(info.InventoryNumber))
				throw new InvalidOperationException("Device with specified inventory number already exists");

			// Get N/A cabinet in N/A housing to put it as device's location
			var defaultCabinetID = _repo.GetSingle<Cabinet>(
				c => c.Name == "N/A"
			).ID;

			if (!_devicesProvider.DoesDeviceTypeExist(info.TypeID))
				throw new ArgumentException("There is no device type with specified ID");

			var newDevice = FormDeviceFrom(info);

			_repo.Add<Device>(newDevice);
			_repo.Save();

			// Assign N/A cabinet to newly created device
			_repo.Add<DeviceCabinet>(
				new DeviceCabinet {
					DeviceID = newDevice.ID,
					CabinetID = defaultCabinetID
				}
			);
			_repo.Save();
		}

		public void UpdateDevice(string targetInventoryNumber, DeviceInfo info)
		{
			if (!_devicesProvider.DoesDeviceExist(info.InventoryNumber))
				throw new InvalidOperationException("There is no device with specified inventory number");

			var targetDevice = _repo.GetSingle<Device>(
				d => d.InventoryNumber == targetInventoryNumber);

			if (info.TypeID != Guid.Empty)
			{
				if (!_devicesProvider.DoesDeviceTypeExist(info.TypeID))
					throw new InvalidOperationException("There is no device type with specified ID");

				targetDevice.TypeID = info.TypeID;
			}

			if (info.InventoryNumber != null) {
				if (_devicesProvider.DoesDeviceExist(info.InventoryNumber))
					throw new ArgumentException("There is already device with specified inventory number");

				targetDevice.InventoryNumber = info.InventoryNumber;
			}

			if (info.NetworkName != null) {
				if (!_devicesProvider.IsNetworkNameUnique(info.NetworkName))
					throw new ArgumentException("There is already device with specified network name");

				targetDevice.NetworkName = info.NetworkName;
			}

			_repo.Update<Device>(targetDevice);
			_repo.Save();
		}

		public void DeleteDevice(string inventoryNumber)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new InvalidOperationException("There is no device with specified inventory number");

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			_repo.Remove<Device>(targetDevice.ID);
			_repo.Save();
		}

		public void MoveDevice(string inventoryNumber, Guid cabinetID)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new InvalidOperationException("There is no device with specified inventory number");

			var targetCabinet = _repo.GetByID<Cabinet>(cabinetID);
			if (targetCabinet == null)
				throw new ArgumentException("Could not find cabinet with specified ID");

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			var deviceCabinet = _repo.GetSingle<DeviceCabinet>(dc => dc.DeviceID == targetDevice.ID);

			deviceCabinet.CabinetID = cabinetID;

			_repo.Update<DeviceCabinet>(deviceCabinet);
			_repo.Save();

		}

		public void AddSoftware(string inventoryNumber, SoftwareInfo info)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new InvalidOperationException("There is no device with specified ID");

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			// Check if device already has software with the same name AND version
			if (_devicesProvider.HasSoftware(targetDevice.ID, info.SoftwareName, info.SoftwareVersion))
				throw new InvalidOperationException("Specified device already has software with specified version");

			var newDeviceSoftware = new DeviceSoftware {
				DeviceID = targetDevice.ID,
				SoftwareName = info.SoftwareName,
				SoftwareVersion = info.SoftwareVersion
			};

			_repo.Add<DeviceSoftware>(newDeviceSoftware);
			_repo.Save();
		}

		public void RemoveSoftware(string inventoryNumber, SoftwareInfo info)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new InvalidOperationException("There is no device with specified inventory number");

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			if (info.SoftwareVersion != null)
			{
				if (!_devicesProvider.HasSoftware(targetDevice.ID, info.SoftwareName, info.SoftwareVersion))
					throw new InvalidOperationException("There is no software with specified name and version");

				var targetSoftware = _devicesProvider.GetDeviceSoftware(targetDevice.ID, info.SoftwareName, info.SoftwareVersion);

				_repo.Remove<DeviceSoftware>(targetSoftware);
				_repo.Save();
			}
			else
			{
				if (!_devicesProvider.HasSoftware(targetDevice.ID, info.SoftwareName))
					throw new InvalidOperationException("There is no software with specified name");

				IEnumerable<DeviceSoftware> targetSoftware = _devicesProvider.GetDeviceSoftware(targetDevice.ID, info.SoftwareName);

				foreach (var software in targetSoftware)
					_repo.Remove<DeviceSoftware>(software);
				_repo.Save();
			}
		}

		public void AddIPAddress(string inventoryNumber, string ipAddress)
		{

		}

		public void RemoveIPAddress(string inventoryNumber, string ipAddress)
		{

		}

		private Device FormDeviceFrom(DeviceInfo info)
		{
			var newDevice = new Device();

			if (!string.IsNullOrWhiteSpace(info.InventoryNumber))
				newDevice.InventoryNumber = info.InventoryNumber;

			if (!string.IsNullOrWhiteSpace(info.NetworkName))
				newDevice.NetworkName = info.NetworkName;

			if (!string.IsNullOrWhiteSpace(info.ModelName))
				newDevice.ModelName = info.ModelName;

			if (!string.IsNullOrWhiteSpace(info.ModelName))
				newDevice.ModelName = info.ModelName;

			if (info.TypeID != Guid.Empty)
				newDevice.TypeID = info.TypeID;

			return newDevice;
		}
	}
}
