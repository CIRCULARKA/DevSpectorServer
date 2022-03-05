using System;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.Database;
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
				return BadRequest(
					new {
						Error = "Can't add device",
						Description = e.Message
					}
				);
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
				return BadRequest(
					new {
						Error = "Can't update device",
						Description = e.Message
					}
				);
			}
		}

		[HttpDelete("api/devices/remove")]
		[RequireParameters("inventoryNumber")]
		public IActionResult RemoveDevice(string inventoryNumber)
		{
			try
			{
				_devicesManager.DeleteDevice(inventoryNumber);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Could not remove device",
					Description = e.Message
				});
			}

		}

		[HttpGet("api/devices/types")]
		public JsonResult GetDeviceTypes() =>
			Json(_devicesManager.GetDeviceTypes());

		[HttpPut("api/devices/move")]
		[RequireParameters("inventoryNumber", "cabinetID")]
		public IActionResult MoveDevice(string inventoryNumber, Guid cabinetID)
		{
			try
			{
				_devicesManager.MoveDevice(inventoryNumber, cabinetID);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Could not move device",
					Description = e.Message
				});
			}
		}

		[HttpPost("api/devices/assign-software")]
		[RequireParameters("inventoryNumber", "softwareName", "softwareVersion")]
		public IActionResult AssignSoftware(string inventoryNumber, SoftwareInfo newSoftwareInfo)
		{
			try
			{
				_devicesManager.AddSoftware(inventoryNumber, newSoftwareInfo);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Could not add software to device",
					Description = e.Message
				});
			}
		}
	}
}
