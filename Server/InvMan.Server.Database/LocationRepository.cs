using System.Linq;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class LocationRepository : ILocationRepository
	{
		private readonly ApplicationDbContextBase _context;

		public LocationRepository(ApplicationDbContextBase context) =>
			_context = context;

		public IQueryable<Housing> Housings =>
			_context.Housings;

		public IQueryable<HousingCabinets> HousingCabinets =>
			_context.HousingCabinets;
	}
}
