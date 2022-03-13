using DevSpector.Domain;
using DevSpector.Application;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceRegister
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
		{
			@this.AddTransient<IRepository, Repository>();

			@this.AddTransient<IDevicesManager, DevicesManager>();
			@this.AddTransient<ILocationManager, LocationManager>();
			@this.AddTransient<IIPAddressesManager, IPAddressesManager>();

			@this.AddScoped<UsersManager>();

			return @this;
		}
	}
}
