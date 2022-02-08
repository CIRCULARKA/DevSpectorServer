using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using InvMan.Server.UI.Filters;
using InvMan.Server.Domain.Models;
using InvMan.Common.SDK.Models;
using InvMan.Server.Application;

namespace InvMan.Server.UI.API.Controllers
{
	public class ClientUsersController : ApiController
	{
        private readonly ClientUsersManager _usersManager;

		private readonly SignInManager<ClientUser> _signInManager;

		public ClientUsersController(
            ClientUsersManager usersManager,
			SignInManager<ClientUser> signInManager
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
					Select(u => new User(u.AccessKey, u.UserName, u.Group))
			);

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("login", "password", "group")]
        public async Task<IActionResult> CreateUser(string login, string password, string group)
		{
			var newUser = new ClientUser {
				UserName = login,
				AccessKey = Guid.NewGuid().ToString(),
				Group = group
			};

			var result = await _usersManager.CreateAsync(newUser, password);

			if (!result.Succeeded)
				return BadRequest(
					new {
						Error = "User wasn't created",
						CriteriaWerentMet = result.Errors.Select(e => e.Description)
					}
				);

			try { var roleResult = await _usersManager.AddToRoleAsync(newUser, group); }
			catch
			{
				await _usersManager.DeleteAsync(newUser);
				return BadRequest(new { Error = "User wasn't created", Details = "Specified role doesn't exists" });
			}

			return Ok();
		}

		[HttpPost("api/users/remove")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("login")]
		public async Task<IActionResult> RemoveUser(string login)
		{
			var targetUser = await _usersManager.FindByNameAsync(login);

			if (targetUser == null)
				return BadRequest(
					new { Error = "User wasn't deleted", Descritpion = "User with specified login doesn't exists" }
				);

			var result = await _usersManager.DeleteAsync(targetUser);

			if (result == null)
				return BadRequest(
					new BadRequestErrorMessage {
						Error = "User not found",
						Description = "User with specified login doesn't exist"
					}
				);

			if (!result.Succeeded)
				return BadRequest(
					new {
						Error = "Can't delete user",
						Description = result.Errors.Select(ie => ie.Description)
					}
				);

			return Ok();
		}

		[HttpGet("api/users/authorize")]
		[RequireParameters("login")]
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

			return Json(
				new {
					Status = "Authorized",
					Login = targetUser.UserName,
					Group = targetUser.Group,
					AccessToken = targetUser.AccessKey
				}
			);
		}
	}
}
