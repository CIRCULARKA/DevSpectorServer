using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class IPAddressRepository : IIPAddressRepository
	{
		private ApplicationDbContextBase _context;

		public IPAddressRepository(ApplicationDbContextBase context)
		{
			_context = context;
		}

		public void UpdateIPs(IEnumerable<IPAddress> newIPs)
		{
			_context.IPAddresses.FromSqlInterpolated($"DELETE * FROM {nameof(_context.IPAddresses)}");
			_context.IPAddresses.AddRange(newIPs);
			_context.SaveChanges();
		}

		public IQueryable<IPAddress> IPAddresses =>
			_context.IPAddresses;

		// public IQueryable<DeviceIPAddresses> DeviceIPAddresses =>
		// 	_context.DeviceIPAddresses;
	}
}
