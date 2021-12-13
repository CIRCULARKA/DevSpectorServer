using System.Linq;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class LocationRepository : ILocationRepository
	{
		private readonly ApplicationDbContext _context;

		public LocationRepository(ApplicationDbContext context) =>
			_context = context;

		public IQueryable<Housing> Housings =>
			_context.Housings;

		public IQueryable<Location> Locations =>
			_context.Locations;
	}
}
