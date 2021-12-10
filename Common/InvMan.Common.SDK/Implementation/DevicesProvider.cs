using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Common.SDK
{
	public class DevicesProvider : IDevicesProvider
	{
		private readonly IRawDataProvider _provider;

		public DevicesProvider(IRawDataProvider provider)
		{
			_provider = provider;
		}

		public Uri Host
		{
			get => _provider.Host;
			set { _provider.Host = value; }
		}

		public async Task<IEnumerable<Appliance>> GetDevicesAsync(int amount) =>
			JsonSerializer.Deserialize<List<Appliance>>(
				await _provider.GetDevicesAsync(amount),
				new JsonSerializerOptions() {
					PropertyNameCaseInsensitive = true
				}
			);
	}
}
