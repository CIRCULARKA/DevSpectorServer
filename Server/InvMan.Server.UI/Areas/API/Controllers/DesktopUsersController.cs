using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using InvMan.Server.UI.Filters;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.UI.API.Controllers
{
	public class DesktopUsersController : ApiController
	{
        private readonly UserManager<DesktopUser> _usersManager;

		private readonly SignInManager<DesktopUser> _signInManager;

		public DesktopUsersController(
            UserManager<DesktopUser> usersManager,
			SignInManager<DesktopUser> signInManager
		)
		{
            _usersManager = usersManager;
			_signInManager = signInManager;
		}

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateUser(string login, string password)
		{
			var newUser = new DesktopUser {
				UserName = login,
				AccessKey = Guid.NewGuid().ToString()
			};

			var result = await _usersManager.CreateAsync(newUser, password);

			if (!result.Succeeded)
				return Json(
					new BadRequestErrorMessage() {
						Error = "User wasn't created",
						Description = result.Errors.Select(e => e.Description)
					}
				);
			else
				return Ok();
		}

		[HttpGet("api/users/authorize")]
		public async Task<IActionResult> AuthorizeUser(string login, string password)
		{
			var targetUser = await _usersManager.FindByNameAsync(login ?? "");

			if (targetUser == null)
				return Unauthorized();

			var result = await _signInManager.PasswordSignInAsync(
				user: targetUser,
				password: password ?? "",
				isPersistent: false,
				lockoutOnFailure: false
			);

			if (!result.Succeeded)
				return Unauthorized();

			HttpContext.Response.Headers.Add("API", targetUser.Id);

			return Json(new { Status = "Authorized", Login = targetUser.UserName });
		}
	}
}
