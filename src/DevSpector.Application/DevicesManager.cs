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

		public void CreateDevice(string networkName, string inventoryNumber, string type)
		{
			var targetTypeID = _repo.GetSingle<DeviceType>(dt => dt.Name == type).ID;
			var defaultLocationID = _repo.GetSingle<Cabinet>(
				include: "Housing",
				filter: l => l.Name == "N/A" && l.Housing.Name == "N/A"
			).ID;

			var newDevice = new Device()
			{
				InventoryNumber = inventoryNumber,
				NetworkName = networkName,
				TypeID = targetTypeID
			};

			_repo.Add<Device>(newDevice);
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
