using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DevSpector.Domain.Models;

namespace DevSpector.Database
{
	public class ApplicationDbContext : IdentityDbContext<ClientUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
			base(options) { }

		public DbSet<DeviceType> DeviceTypes { get; set; }

		public DbSet<Housing> Housings { get; set; }

		public DbSet<Cabinet> Cabinets { get; set; }

		public DbSet<Device> Devices { get; set; }

		public DbSet<DeviceCabinet> DeviceCabinets { get; set; }

		public DbSet<IPAddress> IPAddresses { get; set; }

		protected void ApplyModelConfigurations(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(
				Assembly.GetAssembly(typeof(ModelConfigurationAttribute))
			);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			ApplyModelConfigurations(builder);
		}
	}
}
