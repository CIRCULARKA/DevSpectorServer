using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class LocationRepository : ILocationRepository
	{
		private readonly ApplicationDbContext _context;

		public LocationRepository(ApplicationDbContext context) =>
			_context = context;

		public IEnumerable<Housing> Housings =>
			_context.Housings;

		public IEnumerable<Cabinet> GetHousingCabinets(int housingID) =>
			_context.HousingCabinets.First(hc => hc.Housing.ID == housingID).Cabinets;
	}
}
