using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DevSpector.UI.Filters;
using DevSpector.Application;
using DevSpector.Database.DTO;

namespace DevSpector.UI.API.Controllers
{
	public class UsersController : ApiController
	{
        private readonly UsersManager _usersManager;

		public UsersController(
            UsersManager usersManager,
			SignInManager<DevSpector.Domain.Models.User> signInManager
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
						u => new UserToOutput {
							AccessToken = u.AccessKey,
							Login = u.UserName,
							Group = _usersManager.GetUserGroup(u).Result,
							FirstName = u.FirstName,
							Surname = u.Surname,
							Patronymic = u.Patronymic
						}
					)
			);

        [HttpPost("api/users/create")]
		[ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserToAdd info)
		{
			try
			{
				await _usersManager.CreateUserAsync(info);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось создать нового пользователя",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpPut("api/users/update")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("targetUserLogin")]
		public async Task<IActionResult> UpdateUserAsync(string targetUserLogin, [FromBody] UserToAdd info)
		{
			try
			{
				await _usersManager.UpdateUserAsync(targetUserLogin, info);

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Не удалось обновить пользователя",
					Description = new List<string> { e.Message }
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
				return BadRequest(new BadRequestError {
					Error = "Не удалось удалить пользователя",
					Description = new List<string> { e.Message }
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
					Group = await _usersManager.GetUserGroup(authorizedUser),
					FirstName = authorizedUser.FirstName,
					Surname = authorizedUser.Surname,
					Patronymic = authorizedUser.Patronymic
				});
			}
			catch (Exception e)
			{
				return BadRequest(new BadRequestError {
					Error = "Ошибка авторизации",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpGet("api/users/revoke-key")]
		[RequireParameters("login", "password")]
		public async Task<IActionResult> RevokeUserKey(string login, string password)
		{
			try
			{
				var newKey = await _usersManager.RevokeUserAPIAsync(login, password);

				return Ok(newKey);
			}
			catch (Exception e)
			{
				return Unauthorized(new BadRequestError {
					Error = "Не удалось обновить ключ доступа",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpGet("api/users/change-pwd")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		[RequireParameters("login", "currentPassword", "newPassword")]
		public async Task<IActionResult> ChangePassword(string login, string currentPassword, string newPassword)
		{
			try
			{
				await _usersManager.ChangePasswordAsync(login, currentPassword, newPassword);

				return Ok();
			}
			catch (Exception e)
			{
				return Unauthorized(new BadRequestError {
					Error = "Не удалось изменить пароль",
					Description = new List<string> { e.Message }
				});
			}
		}

		[HttpGet("api/users/groups")]
		[ServiceFilter(typeof(AuthorizationFilter))]
		public JsonResult GetUserGroups() =>
			Json(_usersManager.GetUserGroups().
				Select(r => new {
					ID = r.Id,
					Name = r.Name
				}));
	}
}
