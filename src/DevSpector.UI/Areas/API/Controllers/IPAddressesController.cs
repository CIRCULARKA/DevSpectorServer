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
		private readonly IIPAddressEditor _ipAddressEditor;

		private readonly IIPAddressProvider _ipAddressProvider;

		public IPAddressController(
			IIPAddressProvider ipProvider,
			IIPAddressEditor ipEditor
		)
		{
			_ipAddressProvider = ipProvider;
			_ipAddressEditor = ipEditor;
		}

		[HttpGet("api/ip/free")]
		public JsonResult GetFreeIP(bool sorted) =>
			sorted ? Json(_ipAddressProvider.GetFreeIPSorted()) :
				Json(_ipAddressProvider.GetFreeIP());

		[HttpPut("api/ip/generate")]
		[RequireParameters("networkAddress", "mask")]
		public IActionResult GenerateIPAddresses(string networkAddress, int mask)
		{
			try
			{
				_ipAddressEditor.GenerateRange(networkAddress, mask);

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
