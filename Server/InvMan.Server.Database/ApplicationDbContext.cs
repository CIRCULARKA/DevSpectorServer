using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain.Models;
using InvMan.Server.Database.Configurations;

namespace InvMan.Server.Database
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		public DbSet<DeviceType> DeviceTypes { get; set; }

		public DbSet<Housing> Housings { get; set; }

		public DbSet<Cabinet> Cabinets { get; set; }

		public DbSet<Device> Devices { get; set; }

		public DbSet<IPAddress> IPAddresses { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new DeviceTypeConfiguration());
			builder.ApplyConfiguration(new HousingConfiguration());
			builder.ApplyConfiguration(new CabinetConfiguration());
			builder.ApplyConfiguration(new DeviceConfiguration());
			builder.ApplyConfiguration(new IPAddressConfiguration());
		}
	}
}
