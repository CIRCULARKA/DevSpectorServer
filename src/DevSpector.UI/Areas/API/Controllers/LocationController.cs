using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DevSpector.UI.Filters;
using DevSpector.Application.Location;

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
		public IActionResult GetCabinets(Guid housingID)
		{
			try
			{
				return Json(_manager.GetCabinets(housingID).Select(
					c => new { ID = c.ID, Name = c.Name }
				));
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Can't get cabinets from housing",
					Description = e.Message
				});
			}
		}
	}
}
