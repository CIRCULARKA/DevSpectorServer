using System.Linq;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;
using InvMan.Common.SDK.Models;

namespace InvMan.Server.Application
{
	public class DevicesManager : IDevicesManager
	{
		private readonly IDeviceRepository _devicesRepo;

		private readonly IIPAddressRepository _ipRepo;

		public DevicesManager(IDeviceRepository devicesRepo,
			IIPAddressRepository ipRepo)
		{
			_devicesRepo = devicesRepo;
			_ipRepo = ipRepo;
		}

		public Device GetDeviceByID(int deviceID) =>
			_devicesRepo.Devices.FirstOrDefault(d => d.ID == deviceID);

		public IQueryable<Device> GetDevices(int amount) =>
			_devicesRepo.Devices.
				OrderBy(d => d.InventoryNumber).
					Take(amount);

		public IQueryable<Appliance> GetAppliances(int amount) =>
			GetDevices(amount).Select(d =>
				new Appliance(
					d.ID, d.InventoryNumber, d.Type.Name,
					d.NetworkName, d.Location.Housing.Name,
					d.Location.Cabinet.Name,
					_ipRepo.IPAddresses.
						Where(dip => dip.DeviceID == d.ID).
							Select(dip => dip.Address).ToList()
				)
			);
	}
}
