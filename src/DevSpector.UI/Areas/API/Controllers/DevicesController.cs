using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.Domain.Models;
using DevSpector.UI.Filters;
using DevSpector.SDK.Models;

namespace DevSpector.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class DevicesController : ApiController
	{
		private readonly IDevicesManager _devicesManager;

		public DevicesController(
			IDevicesManager devicesManager
		)
		{
			_devicesManager = devicesManager;
		}

		[HttpGet("api/devices")]
		public JsonResult GetDevices() {
			return Json(_devicesManager.GetAppliances());
		}

		[HttpGet("api/devices/{deviceID}")]
		public JsonResult GetDevice(Guid deviceID) {
			return Json(_devicesManager.GetDeviceByID(deviceID));
		}

		[HttpPost("api/devices/add")]
		public IActionResult CreateDevice([FromBody] Device device)
		{
			try
			{
				_devicesManager.CreateDevice(device);

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
				_devicesManager.UpdateDevice(device);

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
