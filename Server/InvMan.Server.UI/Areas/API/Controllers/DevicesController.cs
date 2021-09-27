using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
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
		public IEnumerable<Appliance> Get() =>
			_repository.AllDevices.Select(d => (Appliance)d);

		[HttpGet("{id}")]
		public Appliance Get(int id) =>
			_repository.GetDeviceByID(id);
	}
}
