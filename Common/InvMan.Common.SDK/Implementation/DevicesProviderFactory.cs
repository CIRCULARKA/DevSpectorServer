using System;

namespace InvMan.Common.SDK.Factories
{
	public class DevicesProviderFactory : IDevicesProviderFactory
	{
		/// <summary>
		/// Creates default DevicesProvider using json parser
		/// </summary>
        public IDevicesProvider CreateDefaultDevicesProvider(Uri hostAddress) =>
            new DevicesProvider(new JsonProvider(hostAddress));
	}
}
