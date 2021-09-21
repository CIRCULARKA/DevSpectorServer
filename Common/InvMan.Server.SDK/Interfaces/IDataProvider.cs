using System.Threading.Tasks;

namespace InvMan.Server.SDK
{
	public interface IDataProvider
	{
		Task<string> GetAllDevicesRaw();

		Task<string> GetDeviceIpsRaw();
	}
}
