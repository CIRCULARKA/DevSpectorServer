using System;
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

		[HttpGet("api/location")]
		public JsonResult Get() =>
			Json(_manager.Housings);
	}
}
