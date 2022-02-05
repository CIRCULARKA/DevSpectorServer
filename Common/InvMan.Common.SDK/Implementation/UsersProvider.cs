using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Common.SDK
{
    public class UsersProvider : IUsersProvider
	{
		private readonly IRawDataProvider _provider;

		public UsersProvider(IRawDataProvider provider)
		{
			_provider = provider;
		}

		public async Task<IEnumerable<User>> GetUsersAsync(string accessToken) =>
			JsonSerializer.Deserialize<List<User>>(
				await _provider.GetUsersAsync(accessToken),
				new JsonSerializerOptions() {
					PropertyNameCaseInsensitive = true
				}
			);
	}
}
