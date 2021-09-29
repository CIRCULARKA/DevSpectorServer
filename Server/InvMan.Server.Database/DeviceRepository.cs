using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class DeviceRepository : IDeviceRepository
	{
		private ApplicationDbContext _context;

		public DeviceRepository(ApplicationDbContext context)
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
			var targetDevice = _context.Devices.Find(deviceID);
			_context.Devices.Remove(targetDevice);

			return _context.SaveChangesAsync();
		}

		public Task<int> UpdateDevice(Device newDevice)
		{
			_context.Devices.Update(newDevice);

			return _context.SaveChangesAsync();
		}

		public IQueryable<Device> Devices =>
			_context.Devices.
				Include(d => d.Location).
					Include(d => d.Location.Housing).
					Include(d => d.Location.Cabinet).
				Include(d => d.Type);
	}
}
