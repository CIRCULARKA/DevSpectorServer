using System;
using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider
	{
		Uri Host { get; }

		Task<string> GetDevicesAsync(string accessToken);

		Task<string> GetFreeIPAsync(string accessToken);

		Task<string> GetHousingsAsync(string accessToken);

		Task<string> GetHousingAsync(Guid housingID, string accessToken);
	}
}
