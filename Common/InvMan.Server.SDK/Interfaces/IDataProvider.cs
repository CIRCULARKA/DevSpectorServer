using System.Threading.Tasks;

namespace InvMan.Server.SDK
{
	public interface IDataProvider
	{
		Task<string> GetAllDevicesRawAsync();

		Task<string> GetDeviceIpsRawAsync(int deviceID);
	}
}
