using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Common.SDK
{
	public interface IUsersProvider
	{
		Task<IEnumerable<User>> GetUsersAsync(string accessToken);
	}
}
