using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	[Route("housings")]
	public class LocationController : ApiController
	{
		private readonly ILocationManager _manager;

		public LocationController(ILocationManager manager) =>
			_manager = manager;

		[HttpGet]
		public IEnumerable<string> Get() =>
			_manager.Housings;

		[HttpGet("{housingID}")]
		public IEnumerable Get(Guid housingID) =>
			_manager.GetCabinets(housingID);
	}
}
