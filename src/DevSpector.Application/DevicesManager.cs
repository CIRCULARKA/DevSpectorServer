using System;
using System.Linq;
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

		public void CreateDevice(Device device)
		{
			// InventoryNumber should be unique for each device
			// So if there is the device with same InventoryNumber then throw the exception
			var sameDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == device.InventoryNumber);
			if (sameDevice != null)
				throw new ArgumentException("Device with the same inventory number already exists");

			// Get N/A cabinet in N/A housing to put it as device's location
			var defaultCabinetID = _repo.GetSingle<Cabinet>(
				c => c.Name == "N/A"
			).ID;

			// Try to get device type from specified ID
			// If no type then throw the exception
			var targetType = _repo.GetSingle<DeviceType>(dt => dt.ID == device.TypeID);
			if (targetType == null)
				throw new ArgumentException("Device type with specified ID wasn't found");

			var newDevice = new Device()
			{
				InventoryNumber = device.InventoryNumber,
				TypeID = targetType.ID,
				NetworkName = device.NetworkName
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

		public void UpdateDevice(Device device)
		{
			// Check for device's persistance in database
			// If there is no such device then trow the exception
			var targetDevice = _repo.Get<Device>(d => d.ID == device.ID);
			if (targetDevice == null)
				throw new ArgumentException("Could not update device with specified ID - no such device");

			_repo.Update<Device>(device);
			_repo.Save();
		}

		public Device GetDeviceByID(Guid deviceID) =>
			_repo.GetByID<Device>(deviceID);

		public IEnumerable<Device> GetDevices() =>
			_repo.Get<Device>(include: "Type");

		public Cabinet GetDeviceCabinet(Guid deviceID) =>
			_repo.GetSingle<DeviceCabinet>(include: "Cabinet,Cabinet.Housing").Cabinet;

		public IEnumerable<Appliance> GetAppliances()
		{
			return GetDevices().Select(d => {
				var deviceCabinet = GetDeviceCabinet(d.ID);
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
	}
}
