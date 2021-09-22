using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvMan.Server.Domain;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.UI.API.Controllers
{
	public class IPAddressController : ApiController
	{
		private IIPAddressRepository _repository;

		public IPAddressController(IIPAddressRepository repo)
		{
			_repository = repo;
		}

		[HttpGet("{id}")]
		public IEnumerable Get(int id) =>
			_repository.GetDeviceIPs(id).Select(ip => ip.Address);

		[HttpGet("free")]
		public IEnumerable<IPAddress> Get() =>
			_repository.FreeAddresses;
	}
}
