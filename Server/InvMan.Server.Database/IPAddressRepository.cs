using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class IPAddressRepository : IIPAddressRepository
	{
		private ApplicationDbContext _context;

		public IPAddressRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<IPAddress> GetDeviceIPs(int deviceID) =>
			_context.IPAddresses.Where(ip => ip.Device.ID == deviceID).ToList();

		public IEnumerable<IPAddress> FreeAddresses =>
			_context.IPAddresses.Where(ip => ip.Device == null).ToList();
	}
}
