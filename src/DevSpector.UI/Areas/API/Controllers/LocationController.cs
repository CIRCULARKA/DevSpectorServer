using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.UI.Filters;

namespace DevSpector.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class LocationController : ApiController
	{
		private readonly ILocationManager _manager;

		public LocationController(ILocationManager manager) =>
			_manager = manager;

		[HttpGet("api/location/housings")]
		public JsonResult GetHousings() =>
			Json(_manager.Housings.Select(
				h => new { ID = h.ID, Name = h.Name }
			));

		[HttpGet("api/location/cabinets")]
		public JsonResult GetCabinets(Guid housingID) =>
			Json(_manager.GetCabinets(housingID).Select(
				c => new { ID = c.ID, Name = c.Name }
			));
	}
}
