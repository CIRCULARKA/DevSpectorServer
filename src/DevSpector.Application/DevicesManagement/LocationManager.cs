using System;
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

		public IEnumerable<Housing> Housings =>
			_repo.Get<Housing>();

		public IEnumerable<Cabinet> GetCabinets(Guid housingID)
		{
			var targetHousing = _repo.GetByID<Housing>(housingID);
			if (targetHousing == null)
				throw new ArgumentException("There is no housing with specified ID");

			return _repo.Get<Cabinet>(
				filter: c => c.HousingID == housingID
			);
		}
	}
}
