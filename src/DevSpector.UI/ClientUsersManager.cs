using System;
using System.Linq;
using System.Collections.Generic;
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

        public ClientUsersManager(
			IRepository repo,
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
