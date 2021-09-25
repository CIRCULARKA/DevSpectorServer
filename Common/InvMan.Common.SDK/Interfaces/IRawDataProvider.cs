using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider
	{
		Task<string> GetDevicesAsync();

		Task<string> GetFreeIPAsync();

		Task<string> GetHousingsAsync();

		Task<string> GetHousingAsync(int housingID);
	}
}
