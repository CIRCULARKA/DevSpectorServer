using Microsoft.AspNetCore.Mvc;
using System.Collections;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	public class DevicesController : ApiController
	{
		private readonly IDevicesManager _manager;

		public DevicesController(IDevicesManager manager)
		{
			_manager = manager;
		}

		[HttpGet("{amount}")]
		public IEnumerable Get(int amount) =>
			_manager.GetAppliances(amount);
	}
}
