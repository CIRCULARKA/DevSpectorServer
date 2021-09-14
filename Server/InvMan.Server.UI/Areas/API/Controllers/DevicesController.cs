using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections;
using InvMan.Server.Domain;

namespace InvMan.Server.UI.API.Controllers
{
	public class DevicesController : ApiController
	{
		IDeviceRepository _repository;

		public DevicesController(IDeviceRepository repo)
		{
			_repository = repo;
		}

		[HttpGet]
		public IEnumerable Get() =>
			_repository.AllDevices.
				Select(d => new {
					InventoryNumber = d.InventoryNumber,
					DeviceType = d.Type.Name,
					NetworkName = d.NetworkName,
					Location = new {
						Housing = d.Location.Housing.Name,
						Cabinet = d.Location.Cabinet.Name
					},
					IpAddresses = d.IPAddresses.Select(ip => ip.Address)
				});

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var result = _repository.GetDeviceByID(id);

			return Json(
				new {
					ID = result.ID,
					InventoryNumber = result.InventoryNumber,
					NetworkName = result.NetworkName,
					Type = result.Type,
					IPAddresses = result.IPAddresses.Select(ip => ip.Address)
				}
			);
		}
	}
}
