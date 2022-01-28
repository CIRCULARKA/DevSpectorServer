using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;
using InvMan.Server.UI.Filters;

namespace InvMan.Server.UI.API.Controllers
{
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class IPAddressController : ApiController
	{
		private readonly IIPAddressesManager _manager;

		public IPAddressController(IIPAddressesManager manager)
		{
			_manager = manager;
		}

		[HttpGet("api/free-ip")]
		public JsonResult GetFreeIP(bool sorted) =>
			sorted ? Json(_manager.GetSortedFreeIP()) :
				Json(_manager.GetFreeIP());
	}
}
