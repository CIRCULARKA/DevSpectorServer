using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections;
using InvMan.Server.Domain;
using InvMan.Common.SDK.Models;

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
			_repository.AllDevices.Select(
				d => new {
					ID = d.ID,
					InventoryNumber = d.InventoryNumber,
					Type = d.Type.Name,
					NetworkName = d.NetworkName,
					Housing = d.Location.Housing.Name,
					Cabinet = d.Location.Cabinet.Name
				}
			);

		[HttpGet("{id}")]
		public Appliance Get(int id)
		{
			var device = _repository.GetDeviceByID(id);
			return new Appliance(
				device.ID, device.InventoryNumber, device.Type.Name,
				device.NetworkName, device.Location.Housing.Name,
				device.Location.Cabinet.Name, null
			);
		}
	}
}
