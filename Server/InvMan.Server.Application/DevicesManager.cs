using System;
using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;
using InvMan.Common.SDK.Models;

namespace InvMan.Server.Application
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
			var defaultLocationID = _repo.GetSingle<Location>(
				filter: l => l.Cabinet.Name == "N/A" && l.Housing.Name == "N/A"
			).ID;

			var newDevice = new Device()
			{
				InventoryNumber = inventoryNumber,
				NetworkName = networkName,
				TypeID = targetTypeID,
				LocationID = defaultLocationID
			};

			_repo.Add<Device>(newDevice);
			_repo.Save();
		}

		public Device GetDeviceByID(Guid deviceID) =>
			_repo.GetByID<Device>(deviceID);

		public IEnumerable<Device> GetDevices() =>
			_repo.Get<Device>(include: "Type,Location,Location.Housing,Location.Cabinet");

		public IEnumerable<Appliance> GetAppliances() =>
			GetDevices().Select(d =>
				new Appliance(
					d.ID, d.InventoryNumber, d.Type.Name,
					d.NetworkName, d.Location.Housing.Name,
					d.Location.Cabinet.Name,
					_repo.Get<IPAddress>(
						filter: ip => ip.DeviceID == d.ID
					).Select(ip => ip.Address)
				)
			);
	}
}
