using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DevSpector.UI.Filters;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;
using DevSpector.Application;

namespace DevSpector.UI.API.Controllers
{
	public class UsersController : ApiController
	{
        private readonly ClientUsersManager _usersManager;

		private readonly SignInManager<ClientUser> _signInManager;

		public UsersController(
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
					Select(
						u => new User(
							u.AccessKey,
							u.UserName,
							u.Group
						)
					)
			);

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("login", "password", "group")]
        public async Task<IActionResult> CreateUser(string login, string password, Guid groupID)
		{
			var newUser = new ClientUser {
				UserName = login,
				AccessKey = Guid.NewGuid().ToString()
			};

			var result = await _usersManager.CreateAsync(newUser, password);

			// if (!result.Succeeded)
			// 	return BadRequest(
			// 		new {
			// 			Error = "User wasn't created",
			// 			CriteriaWerentMet = result.Errors.Select(e => e.Description)
			// 		}
			// 	);

			// try { var roleResult = await _usersManager.AddToRoleAsync(newUser, group); }
			// catch
			// {
			// 	await _usersManager.DeleteAsync(newUser);
			// 	return BadRequest(new { Error = "User wasn't created", Details = "Specified role doesn't exists" });
			// }

			return Ok();
		}

		[HttpDelete("api/users/remove")]
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
		[RequireParameters("login", "password")]
		public async Task<IActionResult> AuthorizeUser(string login, string password)
		{
			var wrongCredentialsResponse = Unauthorized(
				new {
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

		[HttpPut("api/users/revoke-api")]
		[RequireParameters("login", "password")]
		public async Task<IActionResult> RevokeUserApi(string login, string password)
		{
			try
			{
				var newKey = await _usersManager.RevokeUserAPIAsync(login, password);

				return Ok(
					new { NewKey = newKey }
				);
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Could not revoke API",
					Description = e.Message
				});
			}
		}

		[HttpGet("api/users/groups")]
		public JsonResult GetUserGroups() =>
			Json(_usersManager.GetUserGroups().
				Select(r => new {
					ID = r.Id,
					Name = r.Name
				}));
	}
}
