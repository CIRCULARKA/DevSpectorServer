using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;
using DevSpector.Database;

namespace DevSpector.Application
{
	public class DevicesManager : IDevicesManager
	{
		private readonly IRepository _repo;

		private readonly IIPAddressesManager _ipManager;

		public DevicesManager(IRepository repo, IIPAddressesManager ipManager)
		{
			_repo = repo;
			_ipManager = ipManager;
		}

		public void CreateDevice(DeviceInfo info)
		{
			ThrowIfDevice(EntityExistance.Exists, info.InventoryNumber);

			// Get N/A cabinet in N/A housing to put it as device's location
			var defaultCabinetID = _repo.GetSingle<Cabinet>(
				c => c.Name == "N/A"
			).ID;

			ThrowIfDeviceTypeNotExists(info.TypeID);

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
			ThrowIfDevice(EntityExistance.DoesNotExist, targetInventoryNumber);

			var targetDevice = _repo.GetSingle<Device>(
				d => d.InventoryNumber == targetInventoryNumber);

			if (info.TypeID != Guid.Empty)
			{
				ThrowIfDeviceTypeNotExists(info.TypeID);
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

		public void DeleteDevice(string inventoryNumber)
		{
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			_repo.Remove<Device>(targetDevice.ID);
			_repo.Save();
		}

		public void MoveDevice(string inventoryNumber, Guid cabinetID)
		{
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

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
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			// Check if device already has software with the same name AND version
			var existingSoftware = GetDeviceSoftware(targetDevice.ID, info);
			if (existingSoftware != null)
				throw new ArgumentException("Specified device already has software with specified version");

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
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

			var existingSoftware = GetDeviceSoftware(targetDevice.ID, info);
			if (existingSoftware == null)
				throw new ArgumentException("Software with specified name or version does not exist");

			_repo.Remove<DeviceSoftware>(existingSoftware);
			_repo.Save();
		}

		public IEnumerable<Device> GetDevices() =>
			_repo.Get<Device>(include: "Type");

		public Cabinet GetDeviceCabinet(string inventoryNumber) =>
			_repo.GetSingle<DeviceCabinet>(include: "Cabinet,Cabinet.Housing,Device",
				filter: dc => dc.Device.InventoryNumber == inventoryNumber).Cabinet;

		public IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID) =>
			_repo.Get<DeviceSoftware>(
				ds => ds.DeviceID == deviceID
			);

		public IEnumerable<Appliance> GetAppliances()
		{
			return GetDevices().Select(d => {
				var deviceCabinet = GetDeviceCabinet(d.InventoryNumber);
				var deviceSoftware = GetDeviceSoftware(d.ID);
				var deviceIPs = GetIPAddresses(d.ID);
				return new Appliance(
					d.ID,
					d.InventoryNumber,
					d.Type.Name,
					d.NetworkName,
					deviceCabinet.Housing.Name,
					deviceCabinet.Name,
					deviceIPs.Select(ip => ip.Address).ToList(),
					deviceSoftware.Select(
						s => $"{s.SoftwareName} ({s.SoftwareVersion})"
					).ToList()
				);
			});
		}

		public IEnumerable<DeviceType> GetDeviceTypes() =>
			_repo.Get<DeviceType>();

		public IEnumerable<IPAddress> GetIPAddresses(Guid deviceID) =>
			_repo.Get<IPAddress>(
				filter: di => di.DeviceID == deviceID
			);

		public void AddIPAddress(string inventoryNumber, string ipAddress)
		{
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

			ThrowIfIPAddressIsInvalid(ipAddress);

			if (!_ipManager.IsAddressFree(ipAddress))
				throw new InvalidOperationException("Specified IP address is already in use or out of range");

			var targetIP = _repo.GetSingle<IPAddress>(ip => ip.Address == ipAddress);
			var targetDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);
			targetIP.DeviceID = targetDevice.ID;

			_repo.Update<IPAddress>(targetIP);
			_repo.Save();
		}

		public void RemoveIPAddress(string inventoryNumber, string ipAddress)
		{
			ThrowIfDevice(EntityExistance.DoesNotExist, inventoryNumber);

			ThrowIfIPAddressIsInvalid(ipAddress);

			if (!HasIP(inventoryNumber, ipAddress))
				throw new InvalidOperationException("Device doesn't have specified IP address");

			var targetIP = _repo.GetSingle<IPAddress>(ip => ip.Address == ipAddress);
			targetIP.DeviceID = null;

			_repo.Update<IPAddress>(targetIP);
			_repo.Save();
		}

		public void ThrowIfDevice(EntityExistance existance, string inventoryNumber)
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

		private DeviceSoftware GetDeviceSoftware(Guid deviceID, SoftwareInfo info) =>
			_repo.GetSingle<DeviceSoftware>(
				ds => (ds.DeviceID == deviceID) &&
					(ds.SoftwareName == info.SoftwareName) &&
					(ds.SoftwareVersion == info.SoftwareVersion)
			);

		private void ThrowIfIPAddressIsInvalid(string ipAddress)
		{
			if (!_ipManager.MathesIPv4(ipAddress))
				throw new ArgumentException("Specified IP address doesn't match IPv4 pattern");
		}

		private bool HasIP(string inventoryNumber, string ipAddress)
		{
			if (!_ipManager.MathesIPv4(ipAddress))
				throw new ArgumentException("Specified IP address doesn't match IPv4 pattern");

			var ip = _repo.GetSingle<IPAddress>(
				include: "Device",
				filter: ip => (ip.Address == ipAddress) && (ip.Device.InventoryNumber == inventoryNumber)
			);

			return ip != null;
		}
	}
}
