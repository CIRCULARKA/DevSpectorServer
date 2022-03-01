using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application
{
	public class ClientUsersManager : UserManager<ClientUser>
	{
		private IRepository _repository;

		private SignInManager<ClientUser> _signInManager;

        public ClientUsersManager(
			IRepository repo,
			SignInManager<ClientUser> signInManager,
			IUserStore<ClientUser> store,
			IOptions<IdentityOptions> optionsAccessor,
			IPasswordHasher<ClientUser> passwordHasher,
			IEnumerable<IUserValidator<ClientUser>> userValidators,
			IEnumerable<IPasswordValidator<ClientUser>> passwordValidators,
			ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors,
			IServiceProvider services,
			ILogger<UserManager<ClientUser>> logger) :
			base (store, optionsAccessor, passwordHasher, userValidators,
				passwordValidators, keyNormalizer, errors, services, logger)
		{
			_repository = repo;
			_signInManager = signInManager;
		}

		public async Task<string> RevokeUserAPIAsync(string login, string password)
		{
			var wrongCredentialsException = new ArgumentException("Login or password is wrong");

			// If there is no user with specified login then throw the exception
			var targetUser = await this.FindByNameAsync(login);
			if (targetUser == null)
				throw wrongCredentialsException;

			// If user's password is wrong the throw the same exception
			var signInResult = await _signInManager.CheckPasswordSignInAsync(targetUser, password, false);
			if (!signInResult.Succeeded)
				throw wrongCredentialsException;

			// If credentials are OK then update user's access key
			targetUser.AccessKey = Guid.NewGuid().ToString();
			await this.UpdateAsync(targetUser);

			return targetUser.AccessKey;
		}

		public ClientUser FindByApi(string key) =>
			Users.FirstOrDefault(u => u.AccessKey == key);

		public IEnumerable<IdentityRole> GetUserGroups() =>
			_repository.Get<IdentityRole>();

		public IdentityRole GetUserGroup(string groupName) =>
			_repository.
				GetSingle<IdentityRole>(r => r.NormalizedName == groupName.ToUpper());
	}
}
