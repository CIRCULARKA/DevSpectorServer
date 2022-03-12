using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DevSpector.Domain;
using DevSpector.Database;
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
			await ThrowIfUser(EntityExistance.Exists, newUserInfo.Login);

			ThrowIfUserGroupNotExists(newUserInfo.GroupID);

			var newUser = await FormUserFrom(newUserInfo);

			var creationResult = await _baseUsersManager.CreateAsync(newUser, newUserInfo.Password);
			if (!creationResult.Succeeded)
				throw GenerateExceptionFromErrors(creationResult.Errors);

			await ChangeUserGroup(newUserInfo.Login, newUserInfo.GroupID);
			// await _baseUsersManager.AddToRoleAsync(newUser, GetGroup(newUserInfo.GroupID).Name);
		}

		public async Task UpdateUserAsync(string targetUserLogin, UserInfo updatedInfo)
		{
			await ThrowIfUser(EntityExistance.DoesNotExist, targetUserLogin);

			var target = await this.FindByLoginAsync(targetUserLogin);

			if (!string.IsNullOrWhiteSpace(updatedInfo.Login))
				target.UserName = updatedInfo.Login;

			if (!string.IsNullOrWhiteSpace(updatedInfo.FirstName))
				target.FirstName = updatedInfo.FirstName;

			if (!string.IsNullOrWhiteSpace(updatedInfo.Surname))
				target.Surname = updatedInfo.Surname;

			if (!string.IsNullOrWhiteSpace(updatedInfo.Patronymic))
				target.Patronymic = updatedInfo.Patronymic;

			if (updatedInfo.GroupID != Guid.Empty)
				await ChangeUserGroup(targetUserLogin, updatedInfo.GroupID);

			await _baseUsersManager.UpdateAsync(target);
		}

		public async Task ChangeUserGroup(string targetLogin, Guid groupID)
		{
			ThrowIfUserGroupNotExists(groupID);

			await ThrowIfUser(EntityExistance.DoesNotExist, targetLogin);

			var targetUser = await this.FindByLoginAsync(targetLogin);

			var currentGroupName = await GetUserGroup(targetUser);
			if (currentGroupName != null)
				await _baseUsersManager.RemoveFromRoleAsync(targetUser, currentGroupName);

			var targetGroup = GetGroup(groupID);

			await _baseUsersManager.AddToRoleAsync(targetUser, targetGroup.Name);
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

		private async Task ThrowIfUser(EntityExistance existance, string login)
		{
			var existingUser = await _baseUsersManager.FindByNameAsync(login);

			if (existance == EntityExistance.Exists) {
				if (existingUser != null)
					throw new ArgumentException("User with specified login already exists");
			}
			else {
				if (existingUser == null)
					throw new ArgumentException("User with specified login does not exist");
			}
		}

		private void ThrowIfUserGroupNotExists(Guid groupID)
		{
			if (GetGroup(groupID) == null)
				throw new ArgumentException("User group with specified ID doesn't exists");
		}

		private ClientUser FormUserFrom(UserInfo updatedInfo)
		{
			var newUser = new ClientUser {
				AccessKey = Guid.NewGuid().ToString()
			};

			if (!string.IsNullOrWhiteSpace(updatedInfo.Login))
				newUser.UserName = updatedInfo.Login;

			if (!string.IsNullOrWhiteSpace(updatedInfo.FirstName))
				newUser.FirstName = updatedInfo.FirstName;

			if (!string.IsNullOrWhiteSpace(updatedInfo.Surname))
				newUser.Surname = updatedInfo.Surname;

			if (!string.IsNullOrWhiteSpace(updatedInfo.Patronymic))
				newUser.Patronymic = updatedInfo.Patronymic;

			return newUser;
		}

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
