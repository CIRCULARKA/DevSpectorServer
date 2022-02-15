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
		public JsonResult Get() =>
			Json(_manager.Housings);
	}
}
