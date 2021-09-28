using System.Linq;

namespace InvMan.Server.Application
{
	public interface IIPAddressesManager
	{
		IQueryable<string> GetFreeIP(int amount);
	}
}
