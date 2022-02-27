using System;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.Domain.Models;
using DevSpector.UI.Filters;

namespace DevSpector.UI.API.Controllers
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
			return Json(_manager.GetAppliances());
		}

		[HttpPost("api/devices/add")]
		public IActionResult CreateDevice([FromBody] Device device)
		{
			try
			{
				_manager.CreateDevice(device);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(
					new {
						Error = "Can't add device",
						Description = e.Message
					}
				);
			}
		}

		[HttpPut("api/devices/update")]
		public IActionResult UpdateDevice([FromBody] Device device)
		{
			return Ok();
		}
	}
}
