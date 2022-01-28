using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	public class DevicesController : ApiController
	{
		private readonly IDevicesManager _manager;

		private readonly ILogger<DevicesController> _logger;

		public DevicesController(
			IDevicesManager manager,
			ILogger<DevicesController> logger
		)
		{
			_manager = manager;
			_logger = logger;
		}

		[HttpGet("api/devices")]
		public JsonResult GetAppliances() {
			Thread.Sleep(5000);
			return Json(_manager.GetAppliances());
		}


		[HttpPut("api/devices/create")]
		public IActionResult CreateDevice(string networkName, string inventoryNumber, string type)
		{
			try
			{
				_logger.LogInformation(
					$"{nameof(CreateDevice)} called:\n" +
					$"\tNetwork name: {networkName}\n" +
					$"\tInventory number: {inventoryNumber}\n" +
					$"\tType: {type}.\n"
				);

				_manager.CreateDevice(networkName, inventoryNumber, type);

				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);

				return BadRequest(e);
			}
		}
	}
}
