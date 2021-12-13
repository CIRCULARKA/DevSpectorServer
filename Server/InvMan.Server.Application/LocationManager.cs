using System;
using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;

namespace InvMan.Server.Application
{
	public class LocationManager : ILocationManager
	{
		private readonly ILocationRepository _locationRepo;

		public LocationManager(ILocationRepository locationRepo) =>
			_locationRepo = locationRepo;

		public IEnumerable<string> Housings =>
			_locationRepo.Housings.Select(
				h => h.Name
			);

		public IEnumerable<string> GetCabinets(Guid housingID) =>
			_locationRepo.Locations.
				Where(l => l.HousingID == housingID).
					Select(l => l.Cabinet.Name);
	}
}
