using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.Models;

namespace InvMan.Common.SDK
{
	public class DevicesProvider
	{
		private readonly IRawDataProvider _provider;

		public DevicesProvider(IRawDataProvider provider)
		{
			_provider = provider;
		}

		public async Task<IEnumerable<Appliance>> GetDevicesAsync() =>
			JsonSerializer.Deserialize<List<Appliance>>(
				await _provider.GetDevicesAsync(),
				new JsonSerializerOptions() {
					PropertyNameCaseInsensitive = true
				}
			);
	}
}
