using System;
using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider : IProvider
	{
		Task<string> GetDevicesAsync();

		Task<string> GetFreeIPAsync();

		Task<string> GetHousingsAsync();

		Task<string> GetHousingAsync(Guid housingID);
	}
}
