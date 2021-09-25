using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Domain;

namespace InvMan.Server.UI.API.Controllers
{
	public class LocationController : ApiController
	{
		private readonly ILocationRepository _repository;

		public LocationController(ILocationRepository repo) =>
			_repository = repo;

		[HttpGet("/housings/")]
		public IEnumerable<string> Get() =>
			_repository.Housings.Select(h => h.Name);

		[HttpGet("{housingID}")]
		public IEnumerable<string> Get(int housingID) =>
			_repository.GetHousingCabinets(housingID).Select(c => c.Name);
	}
}
