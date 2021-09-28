using System.Linq;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Application
{
	public class DevicesManager : IDevicesManager
	{
		private readonly IDeviceRepository _devicesRepo;

		public DevicesManager(IDeviceRepository devicesRepo)
		{
			_devicesRepo = devicesRepo;
		}

		public Device GetDeviceByID(int deviceID) =>
			_devicesRepo.Devices.FirstOrDefault(d => d.ID == deviceID);

		public IQueryable<Device> GetDevices(int take = 0) =>
			_devicesRepo.Devices.Take(take);
	}
}
