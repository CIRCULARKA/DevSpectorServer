using System.Linq;
using InvMan.Server.Domain;

namespace InvMan.Server.Application
{
	public class LocationManager : ILocationManager
	{
		private readonly ILocationRepository _locationRepo;

		public LocationManager(ILocationRepository locationRepo) =>
			_locationRepo = locationRepo;

		public IQueryable<string> Housings =>
			_locationRepo.Housings.Select(
				h => h.Name
			);

		public IQueryable<string> GetCabinets(int housingID) =>
			_locationRepo.HousingCabinets.
				Where(hc => hc.HousingID == housingID).
					Select(hc => hc.Cabinet.Name);
	}
}
