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
		public JsonResult GetDevices() {
			return Json(_manager.GetDevices());
		}

		[HttpGet("api/devices/{deviceID}")]
		public JsonResult GetDevice(Guid deviceID) {
			return Json(_manager.GetDeviceByID(deviceID));
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
				return Json(BadRequest(
					new {
						Error = "Can't add device",
						Description = e.Message
					}
				));
			}
		}

		[HttpPut("api/devices/update")]
		public IActionResult UpdateDevice([FromBody] Device device)
		{
			try
			{
				_manager.UpdateDevice(device);

				return Ok();
			}
			catch (Exception e)
			{
				return Json(BadRequest(
					new {
						Error = "Can't update device",
						Description = e.Message
					}
				));
			}
		}

		[HttpGet("api/devices/types")]
		public JsonResult GetDeviceTypes() =>
			Json(_manager.GetDeviceTypes());
	}
}
