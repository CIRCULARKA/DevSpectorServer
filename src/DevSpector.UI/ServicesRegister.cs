using DevSpector.Domain;
using DevSpector.Application;
using DevSpector.Application.Networking;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceRegister
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<IRepository, Repository>();

			AddDevicesManagementServices(services);
			AddUserManagementServices(services);
			AddNetworkingServices(services);

			return services;
		}

		private static void AddDevicesManagementServices(IServiceCollection services)
		{
			services.AddTransient<IDevicesManager, DevicesManager>();
			services.AddTransient<ILocationManager, LocationManager>();
		}

		private static void AddUserManagementServices(IServiceCollection services)
		{
			services.AddScoped<UsersManager>();
		}

		private static void AddNetworkingServices(IServiceCollection services)
		{
			services.AddTransient<IIPValidator, IPValidator>();
			services.AddTransient<IIPRangeGenerator, IP4RangeGenerator>();

			services.AddTransient<IIPAddressesManager, IPAddressesManager>();
		}
	}
}
