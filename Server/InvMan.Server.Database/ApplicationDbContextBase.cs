using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain.Models;
using InvMan.Server.Database.Configurations;

namespace InvMan.Server.Database
{
	public abstract class ApplicationDbContextBase : DbContext
	{
		public DbSet<DeviceType> DeviceTypes { get; set; }

		public DbSet<Housing> Housings { get; set; }

		public DbSet<Cabinet> Cabinets { get; set; }

		public DbSet<Location> Locations { get; set; }

		public DbSet<HousingCabinets> HousingCabinets { get; set; }

		public DbSet<Device> Devices { get; set; }

		public DbSet<IPAddress> IPAddresses { get; set; }

		public DbSet<DeviceIPAddresses> DeviceIPAddresses { get; set; }

		protected void ApplyModelConfigurations(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new DeviceTypeConfiguration());
			builder.ApplyConfiguration(new HousingConfiguration());
			builder.ApplyConfiguration(new CabinetConfiguration());
			builder.ApplyConfiguration(new HousingCabinetsConfiguration());
			builder.ApplyConfiguration(new DeviceConfiguration());
			builder.ApplyConfiguration(new IPAddressConfiguration());
			builder.ApplyConfiguration(new LocationConfiguration());
			builder.ApplyConfiguration(new DeviceIPAddressesConfiguration());
		}
	}
}
