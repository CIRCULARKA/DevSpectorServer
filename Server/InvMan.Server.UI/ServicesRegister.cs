using InvMan.Server.Domain;
using InvMan.Server.Database;
using InvMan.Server.Application;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceRegister
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
		{
			@this.AddTransient<IDeviceRepository, DeviceRepository>();
			@this.AddTransient<IIPAddressRepository, IPAddressRepository>();
			@this.AddTransient<ILocationRepository, LocationRepository>();
			@this.AddTransient<IDevicesManager, DevicesManager>();
			@this.AddTransient<ILocationManager, LocationManager>();

			return @this;
		}
	}
}
