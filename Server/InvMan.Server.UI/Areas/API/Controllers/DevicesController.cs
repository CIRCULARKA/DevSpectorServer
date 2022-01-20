using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.UI.API.Controllers
{
	[Route("devices")]
	public class DevicesController : ApiController
	{
		private readonly IDevicesManager _manager;

		public DevicesController(IDevicesManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		public JsonResult GetAppliances() =>
			Json(_manager.GetAppliances());


		[HttpPut("create")]
		public IActionResult CreateDevice(Device device)
		{
			_manager.CreateDevice(device);
			return Accepted();
		}
	}
}
