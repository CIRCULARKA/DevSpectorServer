using System;
using System.Linq;
using System.Text.RegularExpressions;
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
			_repo.Get<Location>(
				filter: l => l.HousingID == housingID,
				include: "Cabinet"
			).Select(l => l.Cabinet.Name).
				OrderBy(n => int.Parse(n));
	}
}
