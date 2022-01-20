using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	[Route("free-ip")]
	public class IPAddressController : ApiController
	{
		private readonly IIPAddressesManager _manager;

		public IPAddressController(IIPAddressesManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		public JsonResult GetFreeIP(bool sorted) =>
			sorted ? Json(_manager.GetSortedFreeIP()) :
				Json(_manager.GetFreeIP());
	}
}
