using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IRawDataProvider
	{
		Task<HttpResponseMessage> GetDevicesAsync();

		Task<HttpResponseMessage> GetFreeIPAsync();

		Task<HttpResponseMessage> GetHousingsAsync();

		Task<HttpResponseMessage> GetHousingAsync(int housingID);
	}
}
