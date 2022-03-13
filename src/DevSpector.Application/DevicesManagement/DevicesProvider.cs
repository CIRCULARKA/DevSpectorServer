using System;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;
using DevSpector.Application.Networking;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application
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

		public IEnumerable<Device> GetDevices() =>
			_repo.Get<Device>(include: "Type");

		public IEnumerable<DeviceType> GetDeviceTypes() =>
			_repo.Get<DeviceType>();

		public IEnumerable<IPAddress> GetIPAddresses(Guid deviceID) =>
			_repo.Get<IPAddress>(
				filter: di => di.DeviceID == deviceID
			);

		public IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID) =>
			_repo.Get<DeviceSoftware>(
				ds => (ds.DeviceID == deviceID)
			);

		public IEnumerable<Appliance> GetDevicesAsAppliances()
		{
			return GetDevices().Select(d => {
				var deviceCabinet = GetDeviceCabinet(d.ID);
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

		public IEnumerable<DeviceSoftware> GetDeviceSoftware(Guid deviceID, string softwareName) =>
			_repo.Get<DeviceSoftware>(
				ds => (ds.DeviceID == deviceID) &&
					(ds.SoftwareName == softwareName)
			);

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

		public bool HasSoftware(Guid deviceID, string softwareName) =>
			GetDeviceSoftware(deviceID, softwareName) != null;

		public bool HasSoftware(Guid deviceID, string softwareName, string softwareVersion) =>
			GetDeviceSoftware(deviceID, softwareName, softwareVersion) != null;

		public bool HasIP(Guid deviceID, string ipAddress)
		{
			if (!_ipValidator.Matches(ipAddress, IPProtocol.Version4))
				throw new ArgumentException("Specified IP address doesn't match IPv4 pattern");

			var deviceIp = _repo.GetSingle<IPAddress>(ip => (ip.DeviceID == deviceID) && (ip.Address == ipAddress));

			return deviceID != null;
		}
	}
}
