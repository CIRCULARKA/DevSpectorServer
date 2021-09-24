using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider
	{
		Task<string> GetAllDevicesRawAsync();

		Task<string> GetFreeIPRawAsync();

		Task<string> GetHousingsRawAsync();
	}
}
