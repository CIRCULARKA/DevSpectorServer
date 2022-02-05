using System;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using InvMan.Server.UI.Filters;
using InvMan.Server.Domain.Models;
using InvMan.Common.SDK.Models;

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

		[HttpGet("api/users")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		public JsonResult GetUsers() =>
			Json(
				_usersManager.Users.
					Select(u => new User(u.Id, u.UserName, u.Group))
			);

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateUser(string login, string password, string group)
		{
			var newUser = new DesktopUser {
				UserName = login,
				AccessKey = Guid.NewGuid().ToString(),
				Group = group
			};

			var result = await _usersManager.CreateAsync(newUser, password);

			if (!result.Succeeded)
				return Json(
					new BadRequestErrorMessage() {
						Error = "User wasn't created",
						Description = result.Errors.Select(e => e.Description)
					}
				);

			return Ok();
		}

		[HttpGet("api/users/authorize")]
		public async Task<IActionResult> AuthorizeUser(string login, string password)
		{
			var wrongCredentialsResponse = Unauthorized(
				new BadRequestErrorMessage {
					Error = "Authorization failed",
					Description = "Authorization wasn't completed - wrong credentials"
				});
			var targetUser = await _usersManager.FindByNameAsync(login ?? "");

			if (targetUser == null)
				return wrongCredentialsResponse;

			var result = await _signInManager.PasswordSignInAsync(
				user: targetUser,
				password: password ?? "",
				isPersistent: false,
				lockoutOnFailure: false
			);

			if (!result.Succeeded)
				return wrongCredentialsResponse;

			HttpContext.Response.Headers.Add("API", targetUser.Id);

			return Json(
				new {
					Status = "Authorized",
					Login = targetUser.UserName
				}
			);
		}
	}
}
