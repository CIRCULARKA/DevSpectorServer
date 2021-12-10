using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider : IProvider
	{
		Task<string> GetDevicesAsync(int amount);

		Task<string> GetFreeIPAsync(int amount);

		Task<string> GetHousingsAsync();

		Task<string> GetHousingAsync(int housingID);
	}
}
