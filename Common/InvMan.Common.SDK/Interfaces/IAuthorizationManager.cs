using System.Threading.Tasks;
using InvMan.Common.SDK.Models;

namespace InvMan.Common.SDK.Authorization
{
	public interface IAuthorizationManager
	{
        Task<User> TrySignIn(string login, string password);
    }
}
