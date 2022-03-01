using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application
{
	public class ClientUsersManager
	{
		private IRepository _repository;

		private SignInManager<ClientUser> _signInManager;

		private UserManager<ClientUser> _baseUsersManager;

        public ClientUsersManager(
			IRepository repo,
			SignInManager<ClientUser> signInManager,
			UserManager<ClientUser> baseUsersManager)
		{
			_repository = repo;
			_signInManager = signInManager;
			_baseUsersManager = baseUsersManager;
		}

		public async Task<string> GetUserGroup(ClientUser user) =>
			(await _baseUsersManager.GetRolesAsync(user)).FirstOrDefault();

		public async Task CreateUserAsync(UserInfo newUserInfo)
		{
			// Check if there is user already exists
			var existingUser = await _baseUsersManager.FindByNameAsync(newUserInfo.Login);
			if (existingUser != null)
				throw new ArgumentException("User with specified login already exists");

			// Check if group with specified ID exists
			var existingGroup = GetGroup(newUserInfo.GroupID);
			if (existingGroup == null)
				throw new ArgumentException("There is no user group with specified ID");

			var newUser = new ClientUser {
				UserName = newUserInfo.Login,
				AccessKey = Guid.NewGuid().ToString(),
				FirstName = newUserInfo.FirstName,
				Surname = newUserInfo.Surname,
				Patronymic = newUserInfo.Patronymic
			};

			var creationResult = await _baseUsersManager.CreateAsync(newUser, newUserInfo.Password);
			if (!creationResult.Succeeded)
				throw GenerateExceptionFromErrors(creationResult.Errors);

			await _baseUsersManager.AddToRoleAsync(newUser, existingGroup.Name);
		}

		public async Task DeleteUserAsync(string login)
		{
			// Check if user exists
			var existingUser = await _baseUsersManager.FindByNameAsync(login);
			if (existingUser == null)
				throw new ArgumentException("There is no user with specified login");

			var deletionResult = await _baseUsersManager.DeleteAsync(existingUser);

			if (!deletionResult.Succeeded)
				throw GenerateExceptionFromErrors(deletionResult.Errors);
		}

		/// <summary>
		/// Returns authorized user
		/// </summary>
		public async Task<ClientUser> AuthorizeUser(string login, string password)
		{
			if (login == null)
				throw new ArgumentException("In order to authorize user, user's login can't be null");
			if (password == null)
				throw new ArgumentException("In order to authorize user, user's password can't be null");

			var wrongCredentialsException = new ArgumentException("Login or password is wrong");

			var targetUser = await this.FindByLoginAsync(login);
			if (targetUser == null)
				throw wrongCredentialsException;

			var signInResult = await _signInManager.PasswordSignInAsync(
				user: targetUser,
				password: password,
				isPersistent: false,
				lockoutOnFailure: false
			);

			if (!signInResult.Succeeded)
				throw wrongCredentialsException;

			return targetUser;
		}

		public IEnumerable<ClientUser> GetAllUsers() =>
			_baseUsersManager.Users;

		public async Task<string> RevokeUserAPIAsync(string login, string password)
		{
			var wrongCredentialsException = new ArgumentException("Login or password is wrong");

			// If there is no user with specified login then throw the exception
			var targetUser = await _baseUsersManager.FindByNameAsync(login);
			if (targetUser == null)
				throw wrongCredentialsException;

			// If user's password is wrong the throw the same exception
			var signInResult = await _signInManager.CheckPasswordSignInAsync(targetUser, password, false);
			if (!signInResult.Succeeded)
				throw wrongCredentialsException;

			// If credentials are OK then update user's access key
			targetUser.AccessKey = Guid.NewGuid().ToString();
			await _baseUsersManager.UpdateAsync(targetUser);

			return targetUser.AccessKey;
		}

		public async Task<ClientUser> FindByLoginAsync(string login) =>
			await _baseUsersManager.FindByNameAsync(login);

		public ClientUser FindByApi(string key) =>
			_baseUsersManager.Users.FirstOrDefault(u => u.AccessKey == key);

		public IEnumerable<IdentityRole> GetUserGroups() =>
			_repository.Get<IdentityRole>();

		public IdentityRole GetGroup(Guid id) =>
			_repository.
				GetByID<IdentityRole>(id.ToString());

		public IdentityRole GetGroup(string groupName) =>
			_repository.
				GetSingle<IdentityRole>(r => r.NormalizedName == groupName.ToUpper());

		private ArgumentException GenerateExceptionFromErrors(IEnumerable<IdentityError> errors)
		{
			var builder = new StringBuilder();
			foreach (var error in errors)
				builder.AppendLine(error.Description);

			var result = new ArgumentException($"Some errors have occured:\n{builder.ToString()}");

			return result;
		}
	}
}
