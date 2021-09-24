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

		public async Task<IEnumerable<Appliance>> GetAllDevicesAsync()
		{
			var getDataTask = _provider.GetAllDevicesRawAsync();

			return JsonSerializer.Deserialize<List<Appliance>>(
				await getDataTask,
				new JsonSerializerOptions() {
					PropertyNameCaseInsensitive = true
				}
			);
		}
	}
}
