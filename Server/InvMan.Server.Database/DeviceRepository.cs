using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class DeviceRepository : IDeviceRepository
	{
		private ApplicationDbContextBase _context;

		public DeviceRepository(ApplicationDbContextBase context)
		{
			_context = context;
		}

		public Task<int> CreateDevice(Device newDevice)
		{
			_context.Devices.Add(newDevice);

			return _context.SaveChangesAsync();
		}

		public Task<int> RemoveDevice(int deviceID)
		{
			var targetDevice = GetDeviceByID(deviceID);
			_context.Devices.Remove(targetDevice);

			return _context.SaveChangesAsync();
		}

		public Task<int> UpdateDevice(Device newDevice)
		{
			_context.Devices.Update(newDevice);

			return _context.SaveChangesAsync();
		}

		public Device GetDeviceByID(int deviceID) =>
			_context.Devices.
				Include(d => d.Location).
					Include(d => d.Location.Housing).
					Include(d => d.Location.Cabinet).
				Include(d => d.Type).
					FirstOrDefault(d => d.ID == deviceID);

		public IQueryable<Device> Devices =>
			_context.Devices.
				Include(d => d.Location).
					Include(d => d.Location.Housing).
					Include(d => d.Location.Cabinet).
				Include(d => d.Type);
	}
}
