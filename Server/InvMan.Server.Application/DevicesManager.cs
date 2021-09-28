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

		public IQueryable<Device> GetDevices(int take) =>
			_devicesRepo.Devices.Take(take);

		public IQueryable<Appliance> GetAppliances(int take) =>
			GetDevices(take).Select(d =>
				new Appliance(
				d.ID, d.InventoryNumber, d.Type.Name,
				d.NetworkName, d.Location.Housing.Name,
				d.Location.Cabinet.Name,
				_ipRepo.DeviceIPAddresses.
					Where(dip => dip.DeviceID == d.ID).
						Select(dip => dip.IPAddress.Address).ToList()
				)
			);
	}
}