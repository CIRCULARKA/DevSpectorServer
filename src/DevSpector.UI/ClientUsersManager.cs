using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.SDK.Models;

namespace DevSpector.Application
{
	public class ClientUsersManager : UserManager<ClientUser>
	{
        public ClientUsersManager(
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

		}

		public ClientUser FindByApi(string key) =>
			Users.FirstOrDefault(u => u.AccessKey == key);
	}
}
