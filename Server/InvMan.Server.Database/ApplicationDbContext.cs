using System.Reflection;
using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain.Models;
using InvMan.Server.Database.Configurations;

namespace InvMan.Server.Database
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
			base(options) { }

		public DbSet<DeviceType> DeviceTypes { get; set; }

		public DbSet<Housing> Housings { get; set; }

		public DbSet<Cabinet> Cabinets { get; set; }

		public DbSet<Location> Locations { get; set; }

		public DbSet<Device> Devices { get; set; }

		public DbSet<IPAddress> IPAddresses { get; set; }

		protected void ApplyModelConfigurations(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(
				Assembly.GetAssembly(typeof(ModelConfigurationAttribute))
			);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			ApplyModelConfigurations(builder);
		}
	}
}
