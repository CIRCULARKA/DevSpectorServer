using System;
using System.Linq;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Application
{
	public class LocationManager : ILocationManager
	{
		private IRepository _repo;

		public LocationManager(IRepository repo) =>
			_repo = repo;

		public IEnumerable<string> Housings =>
			_repo.Get<Housing>().Select(
				h => h.Name
			);

		public IEnumerable<string> GetCabinets(Guid housingID) =>
			_repo.Get<Location>(
				filter: l => l.HousingID == housingID
			).Select(l => l.Cabinet.Name);
	}
}
