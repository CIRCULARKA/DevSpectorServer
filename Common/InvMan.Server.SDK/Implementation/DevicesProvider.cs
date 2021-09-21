using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.Models;

namespace InvMan.Server.SDK
{
	public class DevicesProvider
	{
		private readonly IDataProvider _provider;

		public DevicesProvider(IDataProvider provider)
		{
			_provider = provider;
		}

		public async Task<IEnumerable<Device>> GetAllDevicesAsync()
		{
			var getDataTask = _provider.GetAllDevicesRawAsync();

			return JsonSerializer.Deserialize<List<Device>>(
				await getDataTask,
				new JsonSerializerOptions() {
					PropertyNameCaseInsensitive = true
				}
			);
		}
	}
}
