using System;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Database.DTO;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application.Devices
{
	public class DevicesProvider : IDevicesProvider
	{
		private readonly IRepository _repo;

		private readonly IIPValidator _ipValidator;

		public DevicesProvider(
			IRepository repo,
			IIPValidator ipValidator
		)
		{
			_repo = repo;

			_ipValidator = ipValidator;
		}

		public List<Device> GetDevices() =>
			_repo.Get<Device>(include: "Type").ToList();

		public List<DeviceType> GetDeviceTypes() =>
			_repo.Get<DeviceType>().ToList();

		public List<IPAddress> GetIPAddresses(Guid deviceID) =>
			_repo.Get<DeviceIPAddress>(
				filter: di => di.DeviceID == deviceID,
				include: "IPAddress").
				Select(di => di.IPAddress).
					ToList();

		public List<DeviceSoftware> GetDeviceSoftware(Guid deviceID) =>
			_repo.Get<DeviceSoftware>(
				ds => (ds.DeviceID == deviceID)
			).ToList();

		public List<DeviceToOutput> GetDevicesToOutput()
		{
			return GetDevices().Select(d => {
				Cabinet deviceCabinet = GetDeviceCabinet(d.ID);
				List<DeviceSoftware> deviceSoftware = GetDeviceSoftware(d.ID);
				List<IPAddress> deviceIPs = GetIPAddresses(d.ID);

				return new DeviceToOutput {
					ID = d.ID,
					InventoryNumber =  d.InventoryNumber,
					Type = d.Type.Name,
					NetworkName = d.NetworkName,
					ModelName = d.ModelName,
					Housing = deviceCabinet.Housing.Name,
					Cabinet = deviceCabinet.Name,
					IPAddresses = deviceIPs.Select(ip => ip.Address).ToList(),
					Software = deviceSoftware.Select(
						s => new SoftwareInfo { SoftwareName = s.SoftwareName, SoftwareVersion = s.SoftwareVersion }
					).ToList()
				};
			}).ToList();
		}

		public List<DeviceSoftware> GetDeviceSoftware(Guid deviceID, string softwareName) =>
			_repo.Get<DeviceSoftware>(
				ds => (ds.DeviceID == deviceID) &&
					(ds.SoftwareName == softwareName)
			).ToList();

		public Cabinet GetDeviceCabinet(Guid deviceID) =>
			_repo.GetSingle<DeviceCabinet>(include: "Cabinet,Cabinet.Housing",
				filter: dc => dc.DeviceID == deviceID).Cabinet;

		public DeviceSoftware GetDeviceSoftware(Guid deviceID, string softwareName, string softwareVersion) =>
			_repo.GetSingle<DeviceSoftware>(
				ds => (ds.DeviceID == deviceID) &&
					(ds.SoftwareName == softwareName) &&
					(ds.SoftwareVersion == softwareVersion)
			);

		public Device GetDevice(string inventoryNumber) =>
			_repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);

        public bool DoesDeviceExist(string inventoryNumber) =>
			GetDevice(inventoryNumber) != null;

        public bool IsNetworkNameUnique(string networkName)
        {
            var existingDevice = _repo.GetSingle<Device>(d => d.NetworkName == networkName);
            return existingDevice == null;
        }

        public bool DoesDeviceTypeExist(Guid typeID) =>
			_repo.GetByID<DeviceType>(typeID) != null;

		public bool HasSoftware(Guid deviceID, string softwareName)
		{
			var soft = GetDeviceSoftware(deviceID, softwareName);
			if (soft == null)
				return false;

			if (soft.Count == 0)
				return false;

			return true;
		}

		public bool HasSoftware(Guid deviceID, string softwareName, string softwareVersion) =>
			GetDeviceSoftware(deviceID, softwareName, softwareVersion) != null;

		public bool HasIP(Guid deviceID, string ipAddress)
		{
			if (!_ipValidator.Matches(ipAddress, IPProtocol.Version4))
				throw new ArgumentException("Указанный IP-адрес не соответствует шаблону IPv4");

			var deviceIp = _repo.GetSingle<DeviceIPAddress>(ip => (ip.DeviceID == deviceID) &&
				(ip.IPAddress.Address == ipAddress));

			return deviceIp != null;
		}
	}
}
