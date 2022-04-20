using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application.Devices;
using DevSpector.Database.DTO;
using DevSpector.UI.Filters;

namespace DevSpector.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class DevicesController : ApiController
	{
		private readonly IDevicesProvider _devicesProvider;

		private readonly IDevicesEditor _devicesEditor;

		public DevicesController(
			IDevicesProvider provider,
			IDevicesEditor editor
		)
		{
			_devicesProvider = provider;
			_devicesEditor = editor;
		}

		[HttpGet("api/devices")]
		public JsonResult GetDevices() {
			return Json(_devicesProvider.GetDevicesToOutput());
		}

		[HttpPost("api/devices/add")]
		public IActionResult CreateDevice([FromBody] DeviceToAdd newDeviceToAdd)
		{
			try
			{
				_devicesEditor.CreateDevice(newDeviceToAdd);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(
					new BadRequestError {
						Error = "Не удалось добавить устройство",
						Description = new List<string> { e.Message }
					}
				);
			}
		}

		[HttpPut("api/devices/update")]
		[RequireParameters("targetInventoryNumber")]
		public IActionResult UpdateDevice(string targetInventoryNumber, [FromBody] DeviceToUdpate updatedInfo)
		{
			try
			{
				_devicesEditor.UpdateDevice(targetInventoryNumber, updatedInfo);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(
					new BadRequestError {
						Error = "Не удалось обновить устройство",
						Description = new List<string> { e.Message }
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
				_devicesEditor.DeleteDevice(inventoryNumber);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось удалить устройство",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpGet("api/devices/types")]
		public JsonResult GetDeviceTypes() =>
			Json(_devicesProvider.GetDeviceTypes());

		[HttpPut("api/devices/move")]
		[RequireParameters("inventoryNumber")]
		public IActionResult MoveDevice(string inventoryNumber, [FromBody] string cabinetID)
		{
			try
			{
				_devicesEditor.MoveDevice(inventoryNumber, new Guid(cabinetID));

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось переместить устройство",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpPut("api/devices/add-software")]
		[RequireParameters("inventoryNumber")]
		public IActionResult AddSoftware(string inventoryNumber, [FromBody] SoftwareInfo newSoftwareInfo)
		{
			try
			{
				_devicesEditor.AddSoftware(inventoryNumber, newSoftwareInfo);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось добавить ПО к устройству",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpPut("api/devices/remove-software")]
		[RequireParameters("inventoryNumber")]
		public IActionResult RemoveSoftware(string inventoryNumber, [FromBody] SoftwareInfo softwareToRemove)
		{
			try
			{
				_devicesEditor.RemoveSoftware(inventoryNumber, softwareToRemove);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось убрать ПО у устройство",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpPut("api/devices/add-ip")]
		[RequireParameters("inventoryNumber")]
		public IActionResult AddIPAddress(string inventoryNumber, [FromBody] string ipAddress)
		{
			try
			{
				_devicesEditor.AddIPAddress(inventoryNumber, ipAddress);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось добавить IP-адрес к устройству",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpPut("api/devices/remove-ip")]
		[RequireParameters("inventoryNumber")]
		public IActionResult RemoveIPAddress(string inventoryNumber, [FromBody] string ipAddress)
		{
			try
			{
				_devicesEditor.RemoveIPAddress(inventoryNumber, ipAddress);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось удалить IP-адрес устройства",
					Description = new List<string> { e.Message }
				});
			}
		}
	}
}
