using System.Threading.Tasks;

namespace InvMan.Common.SDK.Authorization
{
	public interface IAuthorizationManager
	{
        Task<string> GetAccessTokenAsync(string login, string password);
    }
}
