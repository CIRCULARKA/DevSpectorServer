using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IDataProvider
	{
		Task<string> GetAllDevicesRawAsync();

		Task<string> GetFreeIPRawAsync();

		Task<string> GetHousingsRawAsync();
	}
}
