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

		[HttpGet("api/free-ip")]
		public JsonResult GetFreeIP(bool sorted) =>
			sorted ? Json(_manager.GetSortedFreeIP()) :
				Json(_manager.GetFreeIP());
	}
}
