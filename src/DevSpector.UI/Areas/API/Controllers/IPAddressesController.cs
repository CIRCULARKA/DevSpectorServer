using Microsoft.AspNetCore.Mvc;
using DevSpector.Application;
using DevSpector.UI.Filters;

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
	}
}
