using System;
using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.UI.Filters;
using DevSpector.Application.Networking;

namespace DevSpector.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class IPAddressController : ApiController
	{
		private readonly IIPAddressesManager _manager;

		public IPAddressController(IIPAddressesManager manager)
		{
			_manager = manager;
		}

		[HttpGet("api/ip/free")]
		public JsonResult GetFreeIP(bool sorted) =>
			sorted ? Json(_manager.GetSortedFreeIP()) :
				Json(_manager.GetFreeIP());

		[HttpPut("api/ip/generate")]
		[RequireParameters("networkAddress", "mask")]
		public IActionResult GenerateIPAddresses(string networkAddress, int mask)
		{
			try
			{
				_manager.GenerateRange(networkAddress, mask);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Could not generate new range of IP addresses",
					Description = e.Message
				});
			}
		}
	}
}
