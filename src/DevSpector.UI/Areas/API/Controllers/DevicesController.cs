using System;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.UI.Filters;

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

		[HttpPost("api/devices/add")]
		public IActionResult CreateDevice([FromBody] DeviceInfo newDeviceInfo)
		{
			try
			{
				_devicesManager.CreateDevice(newDeviceInfo);

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
		[RequireParameters("targetInventoryNumber")]
		public IActionResult UpdateDevice(string targetInventoryNumber, [FromBody] DeviceInfo updatedInfo)
		{
			try
			{
				_devicesManager.UpdateDevice(targetInventoryNumber, updatedInfo);

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
			Json(_devicesManager.GetDeviceTypes());
	}
}
