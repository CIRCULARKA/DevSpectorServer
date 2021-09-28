using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	public class LocationController : ApiController
	{
		private readonly ILocationManager _manager;

		public LocationController(ILocationManager manager) =>
			_manager = manager;

		[HttpGet("housings")]
		public IEnumerable<string> Get() =>
			_manager.Housings;

		[HttpGet("housings/{housingID}")]
		public IEnumerable Get(int housingID) =>
			_manager.GetCabinets(housingID);
	}
}
