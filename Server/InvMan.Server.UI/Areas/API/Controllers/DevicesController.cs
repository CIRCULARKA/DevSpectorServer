using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;
using InvMan.Server.UI.Filters;

namespace InvMan.Server.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class DevicesController : ApiController
	{
		private readonly IDevicesManager _manager;

		public DevicesController(
			IDevicesManager manager
		)
		{
			_manager = manager;
		}

		[HttpGet("api/devices")]
		public JsonResult GetAppliances() {
			Thread.Sleep(5000);
			return Json(_manager.GetAppliances());
		}

		[HttpPut("api/devices/create")]
		[RequireParameters("type", "inventoryNumber", "networkName")]
		public IActionResult CreateDevice(string networkName, string inventoryNumber, string type)
		{
			try
			{
				_manager.CreateDevice(networkName, inventoryNumber, type);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}
}
