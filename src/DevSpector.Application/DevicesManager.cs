using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;

namespace DevSpector.Application
{
	public class DevicesManager : IDevicesManager
	{
		private readonly IRepository _repo;

		public DevicesManager(IRepository repo)
		{
			_repo = repo;
		}

		public void CreateDevice(DeviceInfo info)
		{
			// InventoryNumber should be unique for each device
			// So if there is the device with same InventoryNumber then throw the exception
			var sameDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == info.InventoryNumber);
			if (sameDevice != null)
				throw new ArgumentException("Device with the same inventory number already exists");

			// Get N/A cabinet in N/A housing to put it as device's location
			var defaultCabinetID = _repo.GetSingle<Cabinet>(
				c => c.Name == "N/A"
			).ID;

			// Try to get device type from specified ID
			// If no type then throw the exception
			var targetType = _repo.GetSingle<DeviceType>(dt => dt.ID == info.TypeID);
			if (targetType == null)
				throw new ArgumentException("Device type with specified ID wasn't found");

			var newDevice = new Device()
			{
				InventoryNumber = info.InventoryNumber,
				TypeID = targetType.ID,
				NetworkName = info.NetworkName
			};

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

		public void UpdateDevice(DeviceInfo info)
		{
			// Check for device's persistance in database
			// If there is no such device then trow the exception
			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == info.InventoryNumber);
			if (targetDevice == null)
				throw new ArgumentException("Could not update device with specified ID - no such device");

			// Check if specified type is existing if device type ID is specified
			DeviceType targetType;
			if (info.TypeID != Guid.Empty) {
				targetType = _repo.GetByID<DeviceType>(info.TypeID);
				if (targetType == null)
					throw new ArgumentException("Can't assign device type to device - no such type with the specified ID");
				targetDevice.TypeID = info.TypeID;
			}

			if (info.InventoryNumber != null) {
				// Check if there is already device with such inventory number
				var sameDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == info.InventoryNumber);
				if (sameDevice != null)
					throw new ArgumentException("Can't update device - there is already device with inventory number specified");

				targetDevice.InventoryNumber = info.InventoryNumber;
			}

			if (info.NetworkName != null) {
				// Check if there is already device with such network name
				var sameDevice = _repo.GetSingle<Device>(d => d.NetworkName == info.NetworkName);
				if (sameDevice != null)
					throw new ArgumentException("Can't update device - there is already device with network name specified");
				targetDevice.NetworkName = info.NetworkName;
			}

			_repo.Update<Device>(targetDevice);
			_repo.Save();
		}

		public Device GetDeviceByInventoryNumber(string invNum) =>
			_repo.GetSingle<Device>(d => d.InventoryNumber == invNum);

		public IEnumerable<Device> GetDevices() =>
			_repo.Get<Device>(include: "Type");

		public Cabinet GetDeviceCabinet(string inventoryNumber) =>
			_repo.GetSingle<DeviceCabinet>(include: "Cabinet,Cabinet.Housing,Device",
				filter: dc => dc.Device.InventoryNumber == inventoryNumber).Cabinet;

		public IEnumerable<Appliance> GetAppliances()
		{
			return GetDevices().Select(d => {
				var deviceCabinet = GetDeviceCabinet(d.InventoryNumber);
				return new Appliance(
					d.ID,
					d.InventoryNumber,
					d.Type.Name,
					d.NetworkName,
					deviceCabinet.Housing.Name,
					deviceCabinet.Name,
					_repo.Get<IPAddress>(
						filter: ip => ip.DeviceID == d.ID
					).Select(ip => ip.Address).ToList(),
					null
				);
			});
		}

		public IEnumerable<DeviceType> GetDeviceTypes() =>
			_repo.Get<DeviceType>();

		private void ThrowIfDevice(EntityExistance existance, string inventoryNumber)
		{
			var existingDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			if (existance == EntityExistance.Exists) {
				if (existingDevice != null)
					throw new ArgumentException("Device with specified inventory number already exists");
			}
			else {
				if (existingDevice == null)
					throw new ArgumentException("Device with specified inventory number does not exist");
			}
		}

		private void ThrowIfDeviceTypeNotExists(Guid typeID)
		{
			if (_repo.GetByID<DeviceType>(typeID) == null)
				throw new ArgumentException("Device type with specified ID doesn't exists");
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
		private ArgumentException GenerateExceptionFromErrors(IEnumerable<string> errors)
		{
			var builder = new StringBuilder();
			foreach (var error in errors)
				builder.Append($"{error}; ");

			var result = new ArgumentException($"Some errors have occured: {builder.ToString()}");

			return result;
		}
	}
}
