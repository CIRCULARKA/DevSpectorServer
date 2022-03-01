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

		public UsersController(
            ClientUsersManager usersManager,
			SignInManager<ClientUser> signInManager
		)
		{
            _usersManager = usersManager;
		}

		[HttpGet("api/users")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		public JsonResult GetUsers() =>
			Json(
				_usersManager.GetAllUsers().
					Select(
						async u => new User(
							u.AccessKey,
							u.UserName,
							await _usersManager.GetUserGroup(u)
						)
					)
			);

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("login", "password", "groupID")]
        public async Task<IActionResult> CreateUserAsync([FromBody] string login, [FromBody] string password, [FromBody] Guid groupID)
		{
			try
			{
				await _usersManager.CreateUserAsync(login, password, groupID);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Failed to create new user",
					Description = e.Message
				});
			}
		}

		[HttpDelete("api/users/remove")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("login")]
		public async Task<IActionResult> RemoveUserAsync(string login)
		{
			try
			{
				await _usersManager.DeleteUserAsync(login);

				return Ok();
			}
			catch (Exception)
			{

				throw;
			}
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
