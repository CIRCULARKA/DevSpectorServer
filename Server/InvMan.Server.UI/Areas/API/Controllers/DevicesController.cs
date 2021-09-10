using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

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
		public IEnumerable<Device> Get() =>
			_repository.AllDevices;

	}
}
