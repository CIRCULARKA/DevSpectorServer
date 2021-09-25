using InvMan.Server.Domain;
using InvMan.Server.Database;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceRegister
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
		{
			@this.AddTransient<IDeviceRepository, DeviceRepository>();
			@this.AddTransient<IIPAddressRepository, IPAddressRepository>();
			@this.AddTransient<ILocationRepository, LocationRepository>();

			return @this;
		}
	}
}
