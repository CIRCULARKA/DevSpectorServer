using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.UI.API.Controllers
{
	public class DesktopUsersController : ApiController
	{
        private readonly UserManager<DesktopUser> _usersManager;

		public DesktopUsersController(
            UserManager<DesktopUser> usersManager
		)
		{
            _usersManager = usersManager;
		}

        [HttpPost("api/users/create")]
        public async Task<IActionResult> CreateUser(string login, string password)
		{
			var newUser = new DesktopUser {
				UserName = login,
				AccessKey = Guid.NewGuid().ToString()
			};

			var result = await _usersManager.CreateAsync(newUser, password);

			if (result.Errors.Count() > 0) return Json(result.Errors);

			return Ok();
		}
	}
}
