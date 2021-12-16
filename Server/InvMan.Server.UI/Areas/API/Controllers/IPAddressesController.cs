using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	public class IPAddressController : ApiController
	{
		private readonly IIPAddressesManager _manager;

		public IPAddressController(IIPAddressesManager manager)
		{
			_manager = manager;
		}

		[HttpGet("free/")]
		public IEnumerable<string> GetFreeIP() =>
			_manager.GetFreeIP();

		[HttpGet("free/sorted")]
		public IEnumerable<string> GetSortedFreeIP() =>
			_manager.GetSortedFreeIP();
	}
}
