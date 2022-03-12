using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DevSpector.UI.Filters;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;
using DevSpector.Application;
using DevSpector.Database;

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
						u => new User(
							u.AccessKey,
							u.UserName,
							_usersManager.GetUserGroup(u).Result
						)
					)
			);

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserInfo info)
		{
			try
			{
				await _usersManager.CreateUserAsync(info);

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

		[HttpPut("api/users/update")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("targetUserLogin")]
		public async Task<IActionResult> UpdateUserAsync(string targetUserLogin, [FromBody] UserInfo info)
		{
			try
			{
				await _usersManager.UpdateUserAsync(targetUserLogin, info);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Couldn't update the user",
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
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Can't delete user",
					Description = e.Message
				});
			}
		}

		[HttpGet("api/users/authorize")]
		[RequireParameters("login", "password")]
		public async Task<IActionResult> AuthorizeUser(string login, string password)
		{
			try
			{
				var authorizedUser = await _usersManager.AuthorizeUser(login, password);
				return Ok(new {
					Login = authorizedUser.UserName,
					AccessToken = authorizedUser.AccessKey,
					Group = await _usersManager.GetUserGroup(authorizedUser)
				});
			}
			catch (Exception e)
			{
				return BadRequest(new {
					Error = "Failed to authorize",
					Description = e.Message
				});
			}
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
