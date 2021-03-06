using System;
using System.Linq;
using System.Collections.Generic;
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
				h => new { HousingID = h.ID, HousingName = h.Name }
			));

		[HttpGet("api/location/cabinets")]
		public IActionResult GetCabinets(Guid housingID)
		{
			try
			{
				return Json(_manager.GetCabinets(housingID).Select(
					c => new { CabinetID = c.ID, CabinetName = c.Name }
				));
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось получить список кабинетов",
					Description = new List<string> { e.Message }
				});
			}
		}
	}
}
