using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DevSpector.UI.Filters;
using DevSpector.Application.Networking;

namespace DevSpector.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class NetworkController : ApiController
	{
		private readonly IIPAddressEditor _ipAddressEditor;

		private readonly IIPAddressProvider _ipAddressProvider;

		public NetworkController(
			IIPAddressProvider ipProvider,
			IIPAddressEditor ipEditor
		)
		{
			_ipAddressProvider = ipProvider;
			_ipAddressEditor = ipEditor;
		}

		[HttpGet("api/ip/free")]
		public JsonResult GetFreeIP(bool sorted)
		{
			if (sorted)
				return Json(
					_ipAddressProvider.GetFreeIPSorted().
						Select(ip => ip.Address)
				);

			return Json(
				_ipAddressProvider.GetFreeIP().
					Select(ip => ip.Address)
			);
		}


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
