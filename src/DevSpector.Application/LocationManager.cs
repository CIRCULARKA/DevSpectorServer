using System;
using System.Linq;
using System.Collections.Generic;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application
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
			_repo.Get<Cabinet>(
				filter: c => c.HousingID == housingID
			).Select(c => c.Name).
				OrderBy(n => int.Parse(n));
	}
}
