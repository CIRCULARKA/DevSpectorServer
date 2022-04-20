using System;
using System.Collections.Generic;
using DevSpector.Database.DTO;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application.Devices
{
	public class DevicesEditor : IDevicesEditor
	{
		private IRepository _repo;

		private IDevicesProvider _devicesProvider;

		private IIPValidator _ipValidator;

		private IIPAddressProvider _ipProvider;

		private string _noDeviceWithSuchID = "устройства с указанным инвентарным номером не существует";

		private string _deviceWithSuchIDExists = "устройство с указанным инвентарным номером уже существует";

		private string _deviceWithSuchNetworkNameExists = "устройство с указанным сетевым именем уже существует";

		private string _noDeviceTypeWithSuchID = "типа устройства с указанным идентификатором не существует";

		private string _invNumCantBeEmpty = "инвентарный номер не может быть пустым";

		private string _noCabinetWithSuchID = "не удалось найти информацию о помещении по указанному идентификатору";

		private string _ipNotValid = "указанный IP-адрес не соответствует шаблону IPv4";

		public DevicesEditor(
			IRepository repo,
			IDevicesProvider devicesProvider,
			IIPValidator ipValidator,
			IIPAddressProvider ipProvider
		)
		{
			_repo = repo;

			_devicesProvider = devicesProvider;
			_ipProvider = ipProvider;

			_ipValidator = ipValidator;
		}

		public void CreateDevice(DeviceToAdd info)
		{
			if (info.InventoryNumber == null)
				throw new ArgumentNullException(_invNumCantBeEmpty);

			if (_devicesProvider.DoesDeviceExist(info.InventoryNumber))
				throw new ArgumentException(_deviceWithSuchIDExists);

			// Get N/A cabinet in N/A housing to put it as device's location
			var defaultCabinetID = _repo.GetSingle<Cabinet>(
				c => c.Name == "N/A"
			).ID;

			if (!_devicesProvider.DoesDeviceTypeExist(info.TypeID))
				throw new ArgumentException(_noDeviceTypeWithSuchID);

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

		public void UpdateDevice(string inventoryNumber, DeviceToUdpate info)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new ArgumentException("");

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			if (info.TypeID != Guid.Empty)
			{
				if (!_devicesProvider.DoesDeviceTypeExist(info.TypeID))
					throw new ArgumentException(_noDeviceTypeWithSuchID);

				targetDevice.TypeID = info.TypeID;
			}

			if (info.InventoryNumber != null) {
				if (_devicesProvider.DoesDeviceExist(info.InventoryNumber))
					throw new ArgumentException(_deviceWithSuchIDExists);

				targetDevice.InventoryNumber = info.InventoryNumber;
			}

			if (info.NetworkName != null) {
				if (!_devicesProvider.IsNetworkNameUnique(info.NetworkName))
					throw new ArgumentException(_deviceWithSuchNetworkNameExists);

				targetDevice.NetworkName = info.NetworkName;
			}

			if (info.ModelName != null) {
				targetDevice.ModelName = info.ModelName;
			}

			_repo.Update<Device>(targetDevice);
			_repo.Save();
		}

		public void DeleteDevice(string inventoryNumber)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new ArgumentException(_noDeviceWithSuchID);

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			_repo.Remove<Device>(targetDevice);
			_repo.Save();
		}

		public void MoveDevice(string inventoryNumber, Guid cabinetID)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new ArgumentException(_noDeviceWithSuchID);

			var targetCabinet = _repo.GetByID<Cabinet>(cabinetID);
			if (targetCabinet == null)
				throw new ArgumentException(_noCabinetWithSuchID);

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			var deviceCabinet = _repo.GetSingle<DeviceCabinet>(dc => dc.DeviceID == targetDevice.ID);

			deviceCabinet.CabinetID = cabinetID;

			_repo.Update<DeviceCabinet>(deviceCabinet);
			_repo.Save();

		}

		public void AddSoftware(string inventoryNumber, SoftwareInfo info)
		{
			if (info.SoftwareName == null)
				throw new ArgumentNullException("название программного обеспечения должно быть указано");

			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new ArgumentException(_noDeviceWithSuchID);

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			// Check if device already has software with the same name AND version
			if (_devicesProvider.HasSoftware(targetDevice.ID, info.SoftwareName, info.SoftwareVersion))
				throw new InvalidOperationException("выбранное устройство уже имеет ПО с указанной версией");

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
				throw new ArgumentException(_noDeviceWithSuchID);

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			if (info.SoftwareVersion != null)
			{
				if (!_devicesProvider.HasSoftware(targetDevice.ID, info.SoftwareName, info.SoftwareVersion))
					throw new ArgumentException("ПО с указанным названием и версией не найдено");

				var targetSoftware = _devicesProvider.GetDeviceSoftware(targetDevice.ID, info.SoftwareName, info.SoftwareVersion);

				_repo.Remove<DeviceSoftware>(targetSoftware);
				_repo.Save();
			}
			else
			{
				if (!_devicesProvider.HasSoftware(targetDevice.ID, info.SoftwareName))
					throw new ArgumentException("ПО с указанным названием не найдено");

				IEnumerable<DeviceSoftware> targetSoftware = _devicesProvider.GetDeviceSoftware(targetDevice.ID, info.SoftwareName);

				foreach (var software in targetSoftware)
					_repo.Remove<DeviceSoftware>(software);
				_repo.Save();
			}
		}

		public void AddIPAddress(string inventoryNumber, string ipAddress)
		{
			if (ipAddress == null)
				throw new ArgumentNullException("IP-адрес должен быть указан");

			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new ArgumentException(_noDeviceWithSuchID);

			if (!_ipValidator.Matches(ipAddress, IPProtocol.Version4))
				throw new ArgumentException(_ipNotValid);

			if (!_ipProvider.IsAddressFree(ipAddress))
				throw new InvalidOperationException("указанный IP-адрес уже занят или вне диапазона");

			var targetIP = _repo.GetSingle<IPAddress>(ip => ip.Address == ipAddress);

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			_repo.Add(new DeviceIPAddress {
				IPAddressID = targetIP.ID,
				DeviceID = targetDevice.ID
			});
			_repo.Save();
		}

		public void RemoveIPAddress(string inventoryNumber, string ipAddress)
		{
			if (!_devicesProvider.DoesDeviceExist(inventoryNumber))
				throw new ArgumentException(_noDeviceWithSuchID);

			var targetDevice = _devicesProvider.GetDevice(inventoryNumber);

			if (!_ipValidator.Matches(ipAddress, IPProtocol.Version4))
				throw new ArgumentException(_ipNotValid);

			if (!_devicesProvider.HasIP(targetDevice.ID, ipAddress))
				throw new InvalidOperationException("у устройства нет указанного IP-адреса");

			DeviceIPAddress targetIP = _repo.GetSingle<DeviceIPAddress>(di => (di.DeviceID == targetDevice.ID) &&
				(di.IPAddress.Address == ipAddress));

			_repo.Remove(targetIP);
			_repo.Save();
		}

		private Device FormDeviceFrom(DeviceToAdd info)
		{
			var newDevice = new Device();

			if (!string.IsNullOrWhiteSpace(info.InventoryNumber))
				newDevice.InventoryNumber = info.InventoryNumber;

			if (!string.IsNullOrWhiteSpace(info.NetworkName))
				newDevice.NetworkName = info.NetworkName;

			if (!string.IsNullOrWhiteSpace(info.ModelName))
				newDevice.ModelName = info.ModelName;

			if (info.TypeID != Guid.Empty)
				newDevice.TypeID = info.TypeID;

			return newDevice;
		}
	}
}
