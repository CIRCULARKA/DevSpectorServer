using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider
	{
		Task<string> GetDevicesAsync(int amount);

		Task<string> GetFreeIPAsync();

		Task<string> GetHousingsAsync();

		Task<string> GetHousingAsync(int housingID);
	}
}
