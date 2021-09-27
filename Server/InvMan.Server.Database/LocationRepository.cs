using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class LocationRepository : ILocationRepository
	{
		private readonly ApplicationDbContextBase _context;

		public LocationRepository(ApplicationDbContextBase context) =>
			_context = context;

		public IEnumerable<Housing> Housings =>
			_context.Housings;

		public IEnumerable<Cabinet> GetHousingCabinets(int housingID) =>
			_context.HousingCabinets.Where(hc => hc.HousingID == housingID).
				Select(hc => hc.Cabinet);
	}
}
