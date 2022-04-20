using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DevSpector.Domain;
using DevSpector.Database.DTO;
using DevSpector.Domain.Models;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application
{
	// I don't know how to test this class
	public class UsersManager
	{
		private IRepository _repository;

		private SignInManager<User> _signInManager;

		private UserManager<User> _baseUsersManager;

		private string _noUserWithLogin = "пользователя с указанным логином не существует";

		private string _loginOrPasswordWrong = "логин или пароль введены неверно";

        public UsersManager(
			IRepository repo,
			SignInManager<User> signInManager,
			UserManager<User> baseUsersManager)
		{
			_repository = repo;
			_signInManager = signInManager;
			_baseUsersManager = baseUsersManager;
		}

		public async Task<string> GetUserGroup(User user) =>
			(await _baseUsersManager.GetRolesAsync(user)).FirstOrDefault();

		public async Task CreateUserAsync(UserToAdd newUserToAdd)
		{
			await ThrowIfUser(EntityExistance.Exists, newUserToAdd.Login);

			ThrowIfUserGroupNotExists(newUserToAdd.GroupID);

			var newUser = FormUserFrom(newUserToAdd);

			var creationResult = await _baseUsersManager.CreateAsync(newUser, newUserToAdd.Password);
			if (!creationResult.Succeeded)
				throw GenerateExceptionFromErrors(creationResult.Errors);

			await ChangeUserGroup(newUserToAdd.Login, newUserToAdd.GroupID);
		}

		public async Task UpdateUserAsync(string targetUserLogin, UserToUpdate updatedInfo)
		{
			await ThrowIfUser(EntityExistance.DoesNotExist, targetUserLogin);

			var target = await this.FindByLoginAsync(targetUserLogin);

			if (!string.IsNullOrWhiteSpace(updatedInfo.Login)) {
				await ThrowIfUser(EntityExistance.Exists, updatedInfo.Login);
				target.UserName = updatedInfo.Login;
			}

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
				throw new ArgumentException(_noUserWithLogin);

			var deletionResult = await _baseUsersManager.DeleteAsync(existingUser);

			if (!deletionResult.Succeeded)
				throw GenerateExceptionFromErrors(deletionResult.Errors);
		}

		/// <summary>
		/// Returns authorized user
		/// </summary>
		public async Task<User> AuthorizeUser(string login, string password)
		{
			if (login == null)
				throw new ArgumentException("логин не может быть пустым");
			if (password == null)
				throw new ArgumentException("пароль не может быть пустым");

			var wrongCredentialsException = new ArgumentException(_loginOrPasswordWrong);

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

		public IEnumerable<User> GetAllUsers() =>
			_baseUsersManager.Users;

		public async Task<string> RevokeUserAPIAsync(string login, string password)
		{
			var wrongCredentialsException = new ArgumentException(_loginOrPasswordWrong);

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

		public async Task ChangePasswordAsync(string login, string oldPassword, string newPassword)
		{
			var wrongCredentialsException = new ArgumentException(_loginOrPasswordWrong);

			// If there is no user with specified login then throw the exception
			var targetUser = await _baseUsersManager.FindByNameAsync(login);
			if (targetUser == null)
				throw wrongCredentialsException;

			IdentityResult result = await _baseUsersManager.ChangePasswordAsync(targetUser, oldPassword, newPassword);

			if (!result.Succeeded)
				throw wrongCredentialsException;
		}

		public async Task<User> FindByLoginAsync(string login) =>
			await _baseUsersManager.FindByNameAsync(login);

		public User FindByApi(string key) =>
			_baseUsersManager.Users.FirstOrDefault(u => u.AccessKey == key);

		public List<IdentityRole> GetUserGroups() =>
			_repository.Get<IdentityRole>().ToList();

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
					throw new ArgumentException("пользователь с указанным логином уже существует");
			}
			else {
				if (existingUser == null)
					throw new ArgumentException(_noUserWithLogin);
			}
		}

		private void ThrowIfUserGroupNotExists(Guid groupID)
		{
			if (GetGroup(groupID) == null)
				throw new ArgumentException("группа пользователей с указанным идентификатором не существует");
		}

		private User FormUserFrom(UserToAdd updatedInfo)
		{
			var newUser = new User {
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
				builder.Append($"{error.Description} ");

			var result = new ArgumentException($"{builder.ToString()}");

			return result;
		}
	}
}
