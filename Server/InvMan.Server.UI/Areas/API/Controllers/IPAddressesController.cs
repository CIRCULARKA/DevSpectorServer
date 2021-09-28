using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	public class IPAddressController : ApiController
	{
		private readonly IIPAddressesManager _manager;

		public IPAddressController(IIPAddressesManager manager) =>
			_manager = manager;

		[HttpGet("free/${amount}")]
		public IEnumerable<string> Get(int amount) =>
			_manager.GetFreeIP(amount);
	}
}
